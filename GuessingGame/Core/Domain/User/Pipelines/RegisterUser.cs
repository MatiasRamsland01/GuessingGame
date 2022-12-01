using MediatR;
using GuessingGame.Core.Domain.User.Services;

namespace GuessingGame.Core.Domain.User.Pipelines;
public class RegisterUser
{
    public record Request(string email, string password, string username) : IRequest<bool>;
    public class Handler : IRequestHandler<Request, bool>
    {

        private readonly IIdentityService _identityService;
        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        // Returns true if successfully registered user
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            bool result = await _identityService.Register(request.email, request.password, request.username);
            if (result) { return true; }
            return false;
        }
    }
}