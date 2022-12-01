using MediatR;

namespace GuessingGame.Core.Domain.User.Events;

public record GameCreatedEvent : INotification
{
    public GameCreatedEvent(Guid userGuid, string category)
    {
        UserGuid = userGuid;
        Category = category;
    }
    public Guid UserGuid { get; }
    public string Category { get; }
}
