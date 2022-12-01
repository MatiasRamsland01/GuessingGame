using GuessingGame.Core.Domain.Images.DTO;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Core.Domain.Images.Pipelines;

public class GetSegments
{
    public record Request(int imageId) : IRequest<List<ImageDTO>>;

    public class Handler : IRequestHandler<Request, List<ImageDTO>>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<List<ImageDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var images = await _db.Images.Where(c => c.ImageId == request.imageId).ToListAsync(cancellationToken);
            List<ImageDTO> imagesDTO = new();
            foreach (var image in images)
            {
                imagesDTO.Add(new ImageDTO(image.ImageId, image.ImageName, image.SegmentId, image.Segment, image.Category));

            }
            return imagesDTO;

        }
    }
}

