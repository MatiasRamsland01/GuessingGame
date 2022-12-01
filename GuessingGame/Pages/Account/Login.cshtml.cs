using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using GuessingGame.Core.Domain.User.Pipelines;
using GuessingGame.Models;



namespace GuessingGame.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IMediator _mediator;

        public LoginModel(ILogger<LoginModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [BindProperty]
        public LoginInfo Input { get; set; } = null!;

        [TempData]
        public string ErrorMessage { get; set; } = null!;




        public async Task<IActionResult> OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            if (User.Identity is null) {
                throw new NullReferenceException("User.Identity is null");
            }
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/User/IndexAuth");
            }
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Input.Username is null || Input.Password is null)
                {
                    throw new NullReferenceException("The password or username was null!");
                }
                //Sends a request to a pipeline that signs in a user
                var result = await _mediator.Send(new SignInUser.Request(Input.Username, Input.Password, Input.RememberMe));
                if (result)
                {
                    _logger.LogInformation("User logged in.");
                    return Redirect("~/User/IndexAuth");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or password was wrong");
                    return Page();
                }
            }
            return Page();
        }
    }
}
