using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Infrastructure.Data;

//add priority score  for proposing better segments
namespace GuessingGame.Core.Domain.Images.Pipelines;

public class AddPriorityScoreForBetterSegment
{
    public record Request(int? segmentId, int imageId, int score) : IRequest<bool>;

    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));

        }
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            var segmentImg = await _db.Images
                .Where(i => i.ImageId == request.imageId)
                .Where(i => i.SegmentId == request.segmentId)
                .FirstOrDefaultAsync();

            if (segmentImg is null) { throw new NullReferenceException("image is null"); }

            segmentImg.PriorityScoreForBetterSegment += request.score;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}