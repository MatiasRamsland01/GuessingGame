using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Infrastructure.Data;
namespace GuessingGame.Core.Domain.History.Pipelines
{
    public class GetRecentGamesWithUserId
    {
        public record Request(string username) : IRequest<List<History>>;

        public class Handler : IRequestHandler<Request, List<History>>
        {
            private readonly GuessingGameDbContext _db;

            public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

            //Pipeline to get out the 10 games with highest scores for specific user
            public async Task<List<History>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _db.History
                    .Where(d => d.Username == request.username)
                    .OrderByDescending(c => c.GameStateId)
                    .Take(10)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
