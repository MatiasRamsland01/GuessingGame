using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Infrastructure.Data;


namespace GuessingGame.Core.Domain.History.Pipelines;

public class GetLeaderboard
{
    public record Request() : IRequest<List<History>>;

    public class Handler : IRequestHandler<Request, List<History>>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        //Pipeline to get out the 10 games with highest scores
        public async Task<List<History>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _db.History
                .OrderByDescending(c => c.UncoveredSegments)
                .Take(10)
                .ToListAsync(cancellationToken);
        }
    }
}


