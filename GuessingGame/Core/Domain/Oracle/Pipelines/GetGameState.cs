using GuessingGame.Core.Domain.Oracle;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Core.Domain.User.Pipelines;
public class GetGameState
{
    public record Request(Guid UserGuid) : IRequest<GameState>;

    public record Response(GameState State);

    public class Handler : IRequestHandler<Request, GameState?>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<GameState?> Handle(Request request, CancellationToken cancellationToken)
        {
            var state = await _db.GameStates.Include(d => d.ProposedSegmentIds).FirstOrDefaultAsync(gamestate => gamestate.UserGuid == request.UserGuid);

            return state;
        }
    }
}
