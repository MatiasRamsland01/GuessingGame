using System.ComponentModel.DataAnnotations;
using GuessingGame.Core.Domain.Images.Pipelines;
using GuessingGame.Core.Domain.Oracle;
using GuessingGame.Core.Domain.Oracle.Pipelines;
using GuessingGame.Core.Domain.User.Events;
using GuessingGame.Core.Domain.User.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuessingGame.Core.Domain.Oracle.Services;
using System.Text.Json;
using GuessingGame.Core.Domain.History.Pipelines;

namespace GuessingGame.Pages.User
{
    [Authorize]       //must be logged inn
    public class GameViewModel : PageModel
    {
        private readonly ILogger<GameViewModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOracleService _oracleService;

        [BindProperty]
        public int TotalSegmentNum { get; set; }

        [BindProperty]
        public int LeaderboardPlace { get; set; }

        [BindProperty]
        public List<string> Images { get; set; } = null!;

        public List<string> SegmentIdAndImages { get; set; } = null!;

        public string Message { get; set; } = null!;

        public string ProposeNewSegmentsMessage { get; set; } = null!;

        [BindProperty]
        [Required]
        public string Guess { get; set; } = null!;

        public GameState State { get; set; } = null!;
        public string info { get; set; } = null!;

        List<ChoosenSegment> imagesSegments {get; set;} = null!;

        public Guid UserGuid { get; set; }


        public GameViewModel(ILogger<GameViewModel> logger, IMediator mediator, UserManager<IdentityUser> userManager, IOracleService oracle)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _oracleService = oracle;
        }

        // Receives image coordinates via JSON for the user to propose better
        // segments for future users. 
        public async Task<IActionResult> OnPost([FromBody] JsonElement json)
        {

            try
            {
                UserGuid = new Guid(HttpContext.User.Claims.First().Value);
            }
            catch (System.Exception)
            {
                throw new ArgumentException("Failed when trying to convert string guid to class guid");
            }
            State = await _oracleService.ProposeBetterSegments(json, UserGuid);
            return new JsonResult("/User/GameView");
        }

        public async Task OnGetAsync(string message)
        {
            Message = message;
            try
            {
                UserGuid = new Guid(HttpContext.User.Claims.First().Value);
            }
            catch (System.Exception)
            {
                throw new ArgumentException("Failed when trying to convert string guid to class guid");
            }
            State = await _mediator.Send(new GetGameState.Request(UserGuid));

            if (State is null)
            {
                throw new ArgumentNullException("Got null from GetGameStateRequest");
            }
            var guessesLeft = (3 - State.UsedGuesses);

            if (!(State.GameFinished))
            {
                if (guessesLeft == 3 || guessesLeft == 2)
                {
                    info = "black";
                    Message = $"{guessesLeft} guesses left";
                }
                else if (guessesLeft == 1)
                {
                    info = "red";
                    Message = $"1 guess left!";
                }
            }
            else
            {
                LeaderboardPlace = await _mediator.Send(new GetLeaderboardPosition.Request(State.Id));
                ProposeNewSegmentsMessage = $"You have proposed {State.ProposedSegmentIds.Count()} out of {Math.Floor(State.segmentsInImage / 2.0)} segments";
                info = "green";
                Message = $"The correct answer was '{State.CorrectGuess}'.";
            }

            await _mediator.Send(new GetAmountOfSegmentsInImage.Request(State));
            List<ChoosenSegment> imagesSegments = await _mediator.Send(new RetrieveImageSegmentsId.Request(UserGuid));
            Images = await _mediator.Send(new GetImagesWithId.Request(imagesSegments));

            if (TotalSegmentNum == imagesSegments.Count())
            {
                await _mediator.Publish(new GameFinishedEvent(UserGuid, false, State.ShownSegmentsNumber));
                info = "red";
                Message = $"The correct answer was '{State.CorrectGuess}'.";
            }
        }

        public async Task<IActionResult> OnPostSliceAsync(Guid UserGuid)
        {
            await _mediator.Send(new NewImageSegmentInGameState.Request(UserGuid));
            State = await _mediator.Send(new GetGameState.Request(UserGuid));

            if (State.segmentsInImage == State.ChoosenSegments.Count())
            {
                // get whole image from oracle service
                await _mediator.Publish(new GameFinishedEvent(UserGuid, false, State.ShownSegmentsNumber));
                await _mediator.Send(new NewImageSegmentInGameState.Request(UserGuid));
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostGuessAsync(string Guess, int Id)
        {
            try
            {
                UserGuid = new Guid(HttpContext.User.Claims.First().Value);
            }
            catch (System.Exception)
            {
                throw new ArgumentException("Failed when trying to convert string guid to class guid");
            }

            State = await _mediator.Send(new GetGameState.Request(UserGuid));

            await _mediator.Publish(new UserGuessEvent(UserGuid, Guess));
            //Ask oracle to check if guess is correct through a pipeline 
            var was_correct = await _mediator.Send(new VerifyGuess.Request(UserGuid, Guess));
            if (!was_correct)
            {
                var canGuess = await _mediator.Send(new CheckDoneGuessing.Request(UserGuid));
                if (!canGuess)
                {
                    // get whole image from oracle service
                    await _mediator.Publish(new GameFinishedEvent(UserGuid, false, State.ShownSegmentsNumber));
                    await _mediator.Send(new NewImageSegmentInGameState.Request(UserGuid));
                }
            }
            else
            {
                //correct guess
                await _mediator.Publish(new GameFinishedEvent(UserGuid, true, State.ShownSegmentsNumber));
                await _mediator.Send(new NewImageSegmentInGameState.Request(UserGuid));
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostForfeitAsync(Guid UserGuid)
        {
            await _mediator.Publish(new GameForfeitedEvent(UserGuid));
            return Redirect("~/User/IndexAuth");
        }
    }
}
