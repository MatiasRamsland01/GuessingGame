using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Core.Domain.User.Events;

namespace GuessingGame.Core.Domain.History.Handlers;
public class GameFinishedHandlerGame : INotificationHandler<GameFinishedEvent> {

    private readonly GuessingGameDbContext _db;

    public GameFinishedHandlerGame(GuessingGameDbContext db) {
        _db = db ?? throw new System.ArgumentNullException(nameof(db));
    }

    public async Task Handle(GameFinishedEvent notification, CancellationToken cancellationToken) {
        var gamestate = await _db.GameStates.SingleOrDefaultAsync(fi => fi.UserGuid == notification.UserGuid, cancellationToken) ?? throw new Exception($"Gamestate with user id: {notification.UserGuid} was null");
        gamestate.GameWon = notification.Result;
        gamestate.GameFinished = true;
        await _db.SaveChangesAsync(); 
    }
}