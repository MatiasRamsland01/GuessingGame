using Microsoft.AspNetCore.Identity;
namespace GuessingGame.Core.Domain.User.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly UserManager<IdentityUser> _userManager;


        public IdentityService(SignInManager<IdentityUser> signInManager, IUserStore<IdentityUser> userStore, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userStore = userStore;
            _userManager = userManager;
            _emailStore = GetEmailStore();
        }


        // Returns true if successfully logged in user
        public async Task<bool> SignIn(string username, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);
            if (result.Succeeded) { return true; }
            return false;
        }
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();

        }

        // Returns true if successfully registered user
        public async Task<bool> Register(string email, string password, string username)
        {
            var createdUser = CreateUser();
            await _userStore.SetUserNameAsync(createdUser, username, CancellationToken.None);
            await _emailStore.SetEmailAsync(createdUser, email, CancellationToken.None);
            var result = await _userManager.CreateAsync(createdUser, password);
            await SignIn(username, password, false);
            if (result.Succeeded) { return true; }
            return false;
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}