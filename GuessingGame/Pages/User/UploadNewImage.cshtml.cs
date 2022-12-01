using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuessingGame.Models;
using Microsoft.AspNetCore.Mvc;
using GuessingGame.Core.Domain.Images.Services;
using GuessingGame.Core.Domain.Images.Pipelines;


namespace GuessingGame.Pages.User
{
    [Authorize]       //must be logged inn
    public class UploadNewImageModel : PageModel
    {
        private readonly ILogger<GameViewModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        // public Object Image { get; set; }

        private readonly IImageService _imageService;


        public UploadNewImageModel(ILogger<GameViewModel> logger, IMediator mediator, UserManager<IdentityUser> userManager, IImageService imageService)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _imageService = imageService;
        }

        [BindProperty]
        public UploadImageInfoVM uploadImageInfoVM { get; set; } = null!;

        [BindProperty]
        public string errormsg { get; set; } = default!;

        [BindProperty]
        public string msg { get; set; } = default!;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (String.IsNullOrWhiteSpace(uploadImageInfoVM.ImageName))
            {
                ModelState.AddModelError(string.Empty, "Image name cannot be whitespace");
                return Page();
            }
            if (uploadImageInfoVM.Image is null)
            {
                throw new NullReferenceException("uploadImageInfoVM.image was null!");
            }

            if (uploadImageInfoVM.ImageName is null || uploadImageInfoVM.Category is null)
            {
                throw new NullReferenceException("uploadImageInfoVM was null!");
            }

            //check file type
            if (!uploadImageInfoVM.Image.ContentType.StartsWith("image/"))
            {
                throw new FormatException("The uploaded file is not an image");
            }
            if (uploadImageInfoVM.ManualSlice)
            {
                //Checks if the user has drawn onto the canvas and has chosen manual slicing
                if (uploadImageInfoVM.Colors == "")
                {
                    errormsg = "You need to draw ontop of the image for manually slicing";
                }
                else
                {
                    await _mediator.Send(new ManualSliceImages.Request(uploadImageInfoVM.Image, uploadImageInfoVM.SlicedImageBase64, uploadImageInfoVM.Colors, uploadImageInfoVM.ImageName, uploadImageInfoVM.Category));
                }
            }
            else
            {
                await _mediator.Send(new AutomaticallySliceImage.Request(uploadImageInfoVM.Image, uploadImageInfoVM.ImageName, uploadImageInfoVM.Category));
            }
            return RedirectToPage("IndexAuth");
        }
    }
}
