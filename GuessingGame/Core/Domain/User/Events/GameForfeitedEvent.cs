using MediatR;

namespace GuessingGame.Core.Domain.User.Events;

public record GameForfeitedEvent : INotification
{
    public GameForfeitedEvent(Guid userGuid)
    {
        UserGuid = userGuid;
    }
    public Guid UserGuid { get; set; }
}