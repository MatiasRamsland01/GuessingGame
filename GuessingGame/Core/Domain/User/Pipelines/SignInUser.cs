using MediatR;
using GuessingGame.Core.Domain.User.Services;


namespace GuessingGame.Core.Domain.User.Pipelines;
public class SignInUser
{
    public record Request(string username, string password, bool rememberMe) : IRequest<bool>;
    public class Handler : IRequestHandler<Request, bool>
    {

        private readonly IIdentityService _identityService;
        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

         // Returns true if successfully logged in user
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            bool result = await _identityService.SignIn(request.username, request.password, request.rememberMe);
            if (result) { return true; }
            return false;
        }
    }
}