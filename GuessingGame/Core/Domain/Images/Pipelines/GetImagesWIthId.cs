using GuessingGame.Core.Domain.Oracle;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace GuessingGame.Core.Domain.Images.Pipelines;

public class GetImagesWithId
{
    public record Request(List<ChoosenSegment> segments) : IRequest<List<string>>;

    public class Handler : IRequestHandler<Request, List<string>>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<List<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            List<string> imagesBase64 = new List<string>();
            //Converts images to base64 so we can display the images in frontend
            foreach (ChoosenSegment segment in request.segments)
            {
                Image retrievedSegment = await _db.Images.Where(c => c.ImageId == segment.ImageId).Where(d => d.SegmentId == segment.SegmentId).FirstOrDefaultAsync(cancellationToken) ?? throw new Exception($"Could not retrieve segment with segmentId: {segment.SegmentId} and image id: {segment.ImageId}");
                if (retrievedSegment == null)
                {
                    throw new NullReferenceException($"{retrievedSegment} is set to null");
                }
                imagesBase64.Add(Convert.ToBase64String(retrievedSegment.Segment));
            }
            return imagesBase64;
        }
    }
}


