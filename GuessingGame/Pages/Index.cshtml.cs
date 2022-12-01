using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using GuessingGame.Infrastructure.Data;
using GuessingGame.Core.Domain.History.Pipelines;
using GuessingGame.Core.Domain.History;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using GuessingGame.Core.Domain.User.Pipelines;
using GuessingGame.Core.Domain.Images.Pipelines;
using GuessingGame.Models;

namespace GuessingGame.Pages;


public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;
    private readonly GuessingGameDbContext _db;

    [BindProperty]
    public LoginInfo Input { get; set; } = default!;


    [BindProperty(SupportsGet = true)]
    public List<string> Images { get; set; } = null!;

    [TempData]
    public string? ErrorMessage { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IMediator mediator, GuessingGameDbContext db)
    {
        _logger = logger;
        _mediator = mediator;
        _db = db;

    }
    [BindProperty]
    public List<History>? Leaderboard { get; set; } = null!;
    //public string imageDisplay { get; set; }
    public async Task<IActionResult> OnGetAsync(string errorMessage)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, errorMessage);
        }
        if (User.Identity is null)
        {
            throw new NullReferenceException("User.Identity is null");
        }
        if (User.Identity.IsAuthenticated)
        {
            return Redirect("~/User/IndexAuth");
        }

        Random rand = new Random();
        var randomImageId = rand.Next(1, 11);
        Images = await _mediator.Send(new GetFullImageWithImageIdBase64.Request(randomImageId));
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        Leaderboard = await _mediator.Send(new GetLeaderboard.Request());
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            if (Input is null || Input.Username is null || Input.Password is null)
            {
                throw new NullReferenceException("input is null!");
            }

            var result = await _mediator.Send(new SignInUser.Request(Input.Username, Input.Password, Input.RememberMe));
            if (result)
            {
                _logger.LogInformation("User logged in.");
                return Redirect("~/User/IndexAuth");
            }
        }
        ErrorMessage = "Username or password was wrong";
        return RedirectToPage("/Index", new { errorMessage = ErrorMessage });
    }
}

