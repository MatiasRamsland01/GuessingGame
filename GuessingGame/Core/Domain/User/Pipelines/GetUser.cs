using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Core.Domain.User.Pipelines;

public class GetUser
{
    public record Request(Guid UserGuid) : IRequest<IdentityUser>;

    public record Response();

    public class Handler : IRequestHandler<Request, IdentityUser>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public Handler(UserManager<IdentityUser> UserManager)
        {
            _userManager = UserManager ?? throw new System.ArgumentNullException(nameof(UserManager));
        }

        public async Task<IdentityUser> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(c => c.Id == request.UserGuid.ToString()).SingleOrDefaultAsync() ?? throw new Exception($"Could not retrieve user with id: {request.UserGuid}");

            return user;
        }
    }
}
