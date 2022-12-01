namespace GuessingGame.Core.Domain.User.Services
{
    public interface IIdentityService
    {
        public Task<bool> SignIn(string username, string password, bool rememberMe);

        public Task SignOut();

        public Task<bool> Register(string email, string password, string username);
    }
}