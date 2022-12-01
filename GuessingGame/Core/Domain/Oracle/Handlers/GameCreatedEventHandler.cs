using GuessingGame.Core.Domain.Oracle.Services;
using GuessingGame.Core.Domain.User.Events;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Core.Domain.Images.Pipelines;

namespace GuessingGame.Core.Domain.Oracle.Handlers;
public class GameCreatedEventHandler : INotificationHandler<GameCreatedEvent>
{
    private readonly GuessingGameDbContext _db;
    private readonly IMediator _mediator;


    private IOracleService _oracleService;

    public GameCreatedEventHandler(GuessingGameDbContext db, IOracleService oracleService, IMediator mediator)
    {
        _db = db ?? throw new System.ArgumentNullException(nameof(db));
        _oracleService = oracleService;
        _mediator = mediator;
    }

    public async Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
    {
        var item = await _db.GameStates.Include(f => f.ProposedSegmentIds).Include(s => s.ChoosenSegments).SingleOrDefaultAsync(fi => fi.UserGuid == notification.UserGuid);
        if (item is not null)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync(cancellationToken);
        }
        var randomImageId = await _oracleService.GetRandomImageId(notification.Category);
        var segmentIds = await _mediator.Send(new GetSegmentIdsInImage.Request(randomImageId));
        var random = new Random();
        var randomSegmentId = random.Next(segmentIds.Count());
        string imageName = await _mediator.Send(new GetImageName.Request(randomImageId));
        var gameState = new GameState();
        gameState.UserGuid = notification.UserGuid;
        gameState.ImageId = randomImageId;
        gameState.CorrectGuess = imageName;
        gameState.AddChoosenSegment(randomImageId, randomSegmentId);
        await _db.GameStates.AddAsync(gameState);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
