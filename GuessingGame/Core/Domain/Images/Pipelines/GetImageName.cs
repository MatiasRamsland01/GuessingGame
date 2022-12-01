using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace GuessingGame.Core.Domain.Images.Pipelines;

public class GetImageName
{
    public record Request(int imageId) : IRequest<string>;

    public class Handler : IRequestHandler<Request, string>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<string> Handle(Request request, CancellationToken cancellationToken)
        {
            Image image = await _db.Images.Where(c => c.ImageId == request.imageId).FirstOrDefaultAsync(cancellationToken) ?? throw new Exception($"Could not retrieve image with id: {request.imageId}");
            return image.ImageName;
        }
    }
}


