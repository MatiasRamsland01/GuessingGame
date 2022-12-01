using MediatR;

namespace GuessingGame.Core.Domain.User.Events;

//event emitted when the user makes guess
public record UserGuessEvent : INotification
{
    public UserGuessEvent(Guid userGuid, string userGuess)
    {
        UserGuid = userGuid;
        UserGuess = userGuess;
    }
    public Guid UserGuid { get; }
    public string UserGuess { get; }
}
