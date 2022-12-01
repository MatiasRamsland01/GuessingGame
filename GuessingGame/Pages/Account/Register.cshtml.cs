using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuessingGame.Core.Domain.User.Pipelines;
using MediatR;
using GuessingGame.Models;

namespace GuessingGame.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMediator _mediator;

        public RegisterModel(ILogger<RegisterModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;

        }

        [BindProperty]
        public RegisterInfo Input { get; set; } = null!;

        public IActionResult OnGet()
        {
            if (User.Identity is null)
            {
                throw new NullReferenceException("User.Identity is null");
            }
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/User/IndexAuth");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Sending a request to a pipeline that register a new user
                var result = await _mediator.Send(new RegisterUser.Request(Input.Email, Input.Password, Input.Username));

                if (result)
                {
                    return Redirect("~/User/IndexAuth");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Could not create a user.");
                    return Page();
                }
            }
            return Page();
        }
    }
}
