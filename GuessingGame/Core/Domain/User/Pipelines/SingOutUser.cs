using MediatR;
using GuessingGame.Core.Domain.User.Services;

namespace GuessingGame.Core.Domain.User.Pipelines;
public class SignOutUser
{
    public record Request() : IRequest<Unit>;
    public class Handler : IRequestHandler<Request, Unit>
    {

        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

         // Returns true if successfully logged out user
        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            await _identityService.SignOut();
            return Unit.Value;
        }
    }
}