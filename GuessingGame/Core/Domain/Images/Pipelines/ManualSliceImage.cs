using MediatR;
using GuessingGame.Infrastructure.Data;
using GuessingGame.Core.Domain.Images.Services;

namespace GuessingGame.Core.Domain.Images.Pipelines;

public class ManualSliceImages
{

    public record Request(
        IFormFile image,
        string canvasimage,
        string colours,
        string imagename,
        string category) : IRequest<Unit>;

    public class Handler : IRequestHandler<Request, Unit>
    {

        private readonly GuessingGameDbContext _db;
        private readonly IImageService _imageService;

        public Handler(GuessingGameDbContext db, IImageService imageService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            int imageId = _db.Images.Max(d => d.ImageId) + 1;
            List<byte[]> listimages = _imageService.ManualSliceImage(request.canvasimage, request.image, request.colours);
            for (var i = 0; i < listimages.Count(); i++)
            {
                Image createdImage = new Image(imageId, request.imagename, i, listimages[i], request.category);
                _db.Images.Add(createdImage);
            }
            await _db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}