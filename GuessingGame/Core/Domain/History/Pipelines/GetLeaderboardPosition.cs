using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Infrastructure.Data;


namespace GuessingGame.Core.Domain.History.Pipelines;

public class GetLeaderboardPosition
{
    public record Request(int gameStateId) : IRequest<int>;

    public class Handler : IRequestHandler<Request, int>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        //Pipeline to get leaderboard position of a game state
        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            List<History> leaderboard = await _db.History
                                                .OrderByDescending(c => c.UncoveredSegments)
                                                .ToListAsync(cancellationToken);

            var element = leaderboard.Find(c => c.GameStateId == request.gameStateId);
            var index = leaderboard.FindIndex(a => a == element);
            index++;
            return index;
        }
    }
}


