using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GuessingGame.Core.Domain.User.Events;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuessingGame.Pages.User
{
    [Authorize]       //must be logged inn
    public class CategoryChoiceModel : PageModel
    {
        private readonly ILogger<CategoryChoiceModel> _logger;
        private readonly IMediator _mediator;
        //private IIdentityService _identityService;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public string? category { get; set; }
        [BindProperty]
        public Guid userGuid { get; set; }
        public CategoryChoiceModel(ILogger<CategoryChoiceModel> logger, IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        public void OnGet()
        {
            try
            {
                userGuid = new Guid(HttpContext.User.Claims.First().Value);
            }
            catch (System.Exception)
            {
                throw new ArgumentException("Could not convert userguid saved as string to Guid class");
            }

        }
        public async Task<IActionResult> OnPostAsync(string category)
        {
            await _mediator.Publish(new GameCreatedEvent(userGuid, category));
            return RedirectToPage("GameView");
        }
    }
}