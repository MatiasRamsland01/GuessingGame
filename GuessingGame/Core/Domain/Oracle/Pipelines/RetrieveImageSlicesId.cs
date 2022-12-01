using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Core.Domain.Oracle.Pipelines;

public class RetrieveImageSegmentsId
{
    public record Request(Guid UserGuid) : IRequest<List<ChoosenSegment>>;

    public class Handler : IRequestHandler<Request, List<ChoosenSegment>>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<List<ChoosenSegment>> Handle(Request request, CancellationToken cancellationToken)
        {
            var state = await _db.GameStates.Include(cs => cs.ChoosenSegments).FirstOrDefaultAsync(gamestate => gamestate.UserGuid == request.UserGuid);
            if (state is null)
            {
                throw new Exception("GameState was null when RetrieveImageSegmentsId!");
            }
            return state._ChoosenSegments;
        }
    }
}
