using GuessingGame.Core.Domain.Oracle.Services;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Core.Domain.Oracle.Pipelines;
public class NewImageSegmentInGameState
{
    public record Request(Guid userId) : IRequest<Unit>;

    public record Response(bool Success, string[] Errors);

    public class Handler : IRequestHandler<Request, Unit>
    {
        private readonly GuessingGameDbContext _db;
        private readonly IMediator _mediator;
        private readonly IOracleService _oracle;

        public Handler(GuessingGameDbContext db, IMediator mediator, IOracleService oracle)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
            _oracle = oracle;
        }

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var gameState = await _db.GameStates.Include(s => s.ChoosenSegments).FirstOrDefaultAsync(gs => gs.UserGuid == request.userId);
            if (gameState is null) { throw new NullReferenceException("GameState is null when retrieved from db."); }
            if (gameState.GameFinished == false)
            {
                var pieceId = await _oracle.GetRandomPuzzlePieceId(gameState, gameState.ImageId);
                gameState.ShownSegmentsNumber++;
                gameState.AddChoosenSegment(gameState.ImageId, pieceId);
            }
            else
            {
                //Adds the image if game is finished to display all images
                var images = _db.Images
                    .Where(i => i.ImageId == gameState.ImageId).ToList();

                gameState.ClearChoosenSegments();

                foreach (var segment in images)
                {
                    gameState.AddChoosenSegment(gameState.ImageId, segment.SegmentId);
                }
            }
            await _db.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
