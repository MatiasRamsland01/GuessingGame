using GuessingGame.Core.Domain.Oracle.Services;
using GuessingGame.Core.Domain.User.Events;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Core.Domain.Oracle.Handlers;
public class UserGuessEventHandler : INotificationHandler<UserGuessEvent>
{
    private readonly GuessingGameDbContext _db;
    public IOracleService _oracleService;
    public UserGuessEventHandler(GuessingGameDbContext db, IOracleService oracleService)
    {
        _db = db ?? throw new System.ArgumentNullException(nameof(db));
        _oracleService = oracleService;
    }
    public async Task Handle(UserGuessEvent notification, CancellationToken cancellationToken)
    {
        var state = await _db.GameStates.FirstOrDefaultAsync(gamestate => gamestate.UserGuid == notification.UserGuid);
        if (state is null) {
            throw new ArgumentNullException("Got empty state when trying to add guess");
        }
        if (notification.UserGuess == state.CorrectGuess) {
            state.UserCanGuess = false;
        }
        state.UsedGuesses++;
        await _db.SaveChangesAsync(cancellationToken);
    }
}
