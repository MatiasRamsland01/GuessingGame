using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Core.Domain.Images.Pipelines;
public class GetSegmentIdsInImage
{
    public record Request(int imageId) : IRequest<List<int>> { }

    public class Handler : IRequestHandler<Request, List<int>>
    {
        private readonly GuessingGameDbContext _db;

        private readonly IMediator _mediator;

        public Handler(GuessingGameDbContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;

        }

        public async Task<List<int>> Handle(Request request, CancellationToken cancellationToken)
        {
            var segments = await _db.Images.Where(i => i.ImageId == request.imageId).Select(s => s.SegmentId).ToListAsync();
            return segments;
        }
    }
}
