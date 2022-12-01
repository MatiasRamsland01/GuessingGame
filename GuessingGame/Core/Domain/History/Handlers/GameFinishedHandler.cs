using GuessingGame.Infrastructure.Data;
using MediatR;
using GuessingGame.Core.Domain.User.Events;
using Microsoft.AspNetCore.Identity;
using GuessingGame.Core.Domain.Images.Pipelines;
using GuessingGame.Core.Domain.User.Pipelines;
using GuessingGame.Core.Domain.History.Services;

namespace GuessingGame.Core.Domain.History.Handlers;

public class GameFinishedHandler : INotificationHandler<GameFinishedEvent>
{
    private readonly GuessingGameDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMediator _mediator;
    private readonly IHistoryService _historyService;
    public GameFinishedHandler(GuessingGameDbContext db, UserManager<IdentityUser> UserManager, IMediator mediator, IHistoryService historyService)
    {
        _db = db ?? throw new System.ArgumentNullException(nameof(db));
        _userManager = UserManager ?? throw new System.ArgumentNullException(nameof(UserManager));
        _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        _historyService = historyService;
    }

    //Handler that saves the game in the database to be able to have a history of the game
    public async Task Handle(GameFinishedEvent notification, CancellationToken cancellationToken)
    {
        var user = _mediator.Send(new GetUser.Request(notification.UserGuid));
        var username = user.Result.UserName;
        var gamestate = await _mediator.Send(new GetGameState.Request(notification.UserGuid));
        var segmentIds = await _mediator.Send(new GetSegmentIdsInImage.Request(gamestate.ImageId));
        var uncoveredsegments = (segmentIds.Count() - notification.UncoveredSegments);
        var points = _historyService.CalculateScore(segmentIds.Count(), uncoveredsegments, notification.Result);
        var history = new History(gamestate.Id, gamestate.ImageId, points, username);
        _db.History.Add(history);
        await _db.SaveChangesAsync();
    }

}
