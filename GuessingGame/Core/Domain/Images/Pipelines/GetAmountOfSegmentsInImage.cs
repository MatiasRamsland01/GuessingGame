using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Infrastructure.Data;
using GuessingGame.Core.Domain.Oracle;



namespace GuessingGame.Core.Domain.Images.Pipelines;

public class GetAmountOfSegmentsInImage
{
    public record Request(GameState State) : IRequest;

    public class Handler : IRequestHandler<Request>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        //Compares the submitted guess to the correct one
        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            request.State.segmentsInImage = await _db.Images
                .Where(segment=>segment.ImageId==request.State.ImageId)
                .CountAsync();
            
            await _db.SaveChangesAsync(cancellationToken);


            return Unit.Value;
        }
    }
}