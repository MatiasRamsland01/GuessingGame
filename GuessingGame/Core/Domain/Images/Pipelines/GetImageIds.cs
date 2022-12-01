using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Core.Domain.Images.Pipelines;

public class GetImageIds
{
    public record Request(string category) : IRequest<List<int>> { }

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
            if (request.category == "allcategories")
            {
                var ids = await _db.Images.Select(i => i.ImageId).Distinct().ToListAsync();
                if (ids.Count() == 0)
                {
                    throw new Exception("No imageIds!");
                }
                return ids;
            }
            else
            {
                var ids = await _db.Images.Where(k => k.Category == request.category).Select(i => i.ImageId).Distinct().ToListAsync();
                if (ids.Count() == 0)
                {
                    throw new Exception("No imageIds!");
                }
                return ids;
            }
        }
    }
}
