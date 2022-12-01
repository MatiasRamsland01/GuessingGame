using GuessingGame.Core.Domain.History;
using GuessingGame.Core.Domain.History.Pipelines;
using GuessingGame.Core.Domain.Oracle;
using GuessingGame.Core.Domain.User.Pipelines;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace GuessingGame.Pages.User
{
    [Authorize]
    public class IndexAuthModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly GuessingGameDbContext _db;
        public IndexAuthModel(ILogger<IndexModel> logger, IMediator mediator, GuessingGameDbContext db)
        {
            _logger = logger;
            _mediator = mediator;
            _db = db;

        }
        public List<History> Leaderboard { get; set; } = null!;
        public List<History> RecentGames { get; set; } = null!;
        public GameState gameState { get; set; } = null!;
        [BindProperty]
        public Guid userGuid { get; set; }
        [BindProperty]
        public bool gameFinished { get; set; }
        //public string imageDisplay { get; set; }
        public async void OnGetAsync()
        {
            
            try
            {
                userGuid = new Guid(HttpContext.User.Claims.First().Value);
            }
            catch (System.Exception)
            {
                throw new ArgumentException("Could not convert userguid saved as string to Guid class");
            }

            Leaderboard = await _mediator.Send(new GetLeaderboard.Request());
            if (User.Identity is null || User.Identity.Name is null) {
                throw new NullReferenceException("User.Identity.Name is null");
            }
            RecentGames = await _mediator.Send(new GetRecentGamesWithUserId.Request(User.Identity.Name));
            gameState = await _mediator.Send(new GetGameState.Request(userGuid));
            if (gameState == null)
            {
                gameFinished = false;
            }
            else
            {
                gameFinished = gameState.GameFinished;
            }
        }

        public IActionResult OnPostCreateNewGame(Guid userGuid)
        {
            return Redirect("CategoryChoice");
        }

        public async Task<IActionResult> OnPostResumePreviousGameAsync()
        {
            return await Task.Run<ActionResult>(() =>
            {
                return Redirect("GameView");
            });
        }
    }
}
