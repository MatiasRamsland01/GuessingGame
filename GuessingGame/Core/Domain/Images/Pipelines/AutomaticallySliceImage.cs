using MediatR;
using GuessingGame.Infrastructure.Data;
using GuessingGame.Core.Domain.Images.Services;

namespace GuessingGame.Core.Domain.Images.Pipelines;

public class AutomaticallySliceImage
{
    public record Request(IFormFile uploadedImage, string imageName, string category) : IRequest;

    public class Handler : IRequestHandler<Request>
    {
        private readonly GuessingGameDbContext _db;

        private readonly IImageService _imageService;

        public Handler(GuessingGameDbContext db, IImageService imageService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _imageService = imageService;
        }


        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            List<byte[]> retrievedImageSegments = _imageService.SliceImageAutomatic(request.uploadedImage);
            int imageId = _db.Images.Max(d => d.ImageId) + 1;

            //Loops through the desired segments to be able to create every image segment and adding it into the database
            for (var i = 0; i < retrievedImageSegments.Count(); i++)
            {
                GuessingGame.Core.Domain.Images.Image createdImag = new GuessingGame.Core.Domain.Images.Image(imageId, request.imageName, i, retrievedImageSegments[i], request.category);
                _db.Images.Add(createdImag);

            }
            await _db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}