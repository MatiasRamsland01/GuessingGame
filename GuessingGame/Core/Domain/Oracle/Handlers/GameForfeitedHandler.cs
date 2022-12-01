using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Core.Domain.User.Events;

namespace GuessingGame.Core.Domain.History.Handlers;
public class GameForfeitedHandler : INotificationHandler<GameForfeitedEvent>
{

    private readonly GuessingGameDbContext _db;

    public GameForfeitedHandler(GuessingGameDbContext db)
    {
        _db = db ?? throw new System.ArgumentNullException(nameof(db));
    }

    public async Task Handle(GameForfeitedEvent notification, CancellationToken cancellationToken)
    {
        var item = await _db.GameStates.Include(s => s.ChoosenSegments).SingleOrDefaultAsync(fi => fi.UserGuid == notification.UserGuid) ?? throw new ArgumentNullException(nameof(_db)); ;
        _db.Remove(item);
        await _db.SaveChangesAsync(cancellationToken);
    }
}