using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Infrastructure.Data;

namespace GuessingGame.Core.Domain.Images.Pipelines;

public class GetFullImageWithImageIdBase64
{
    public record Request(int imageId) : IRequest<List<string>>;

    public class Handler : IRequestHandler<Request, List<string>>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<List<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            List<string> imagesBase64 = new List<string>();
            List<Image> imageBytes = await _db.Images.Where(c => c.ImageId == request.imageId).ToListAsync(cancellationToken) ?? throw new Exception("Image not found");
            //Converts the image to be able to display it in frontend
            foreach (Image segment in imageBytes)
            {
                imagesBase64.Add(Convert.ToBase64String(segment.Segment));
            }
            return imagesBase64;
        }
    }
}