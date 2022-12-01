using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using GuessingGame.Core.Domain.User.Pipelines;


namespace GuessingGame.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
        private readonly IMediator _mediator;


        public LogoutModel(ILogger<LogoutModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> OnPost()
        {
            //Sending a request to a pipeline to sign out a user
            await _mediator.Send(new SignOutUser.Request());
            _logger.LogInformation("User logged out.");
            return Redirect("~/");
        }
    }
}
