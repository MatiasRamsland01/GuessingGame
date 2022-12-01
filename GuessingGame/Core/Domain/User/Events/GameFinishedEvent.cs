using MediatR;

namespace GuessingGame.Core.Domain.User.Events;

public record GameFinishedEvent : INotification
{
    public GameFinishedEvent(Guid userGuid, bool result, int uncoveredSegments)
    {
        UserGuid = userGuid;
        Result = result;
        UncoveredSegments = uncoveredSegments;

    }
    public Guid UserGuid { get; }
    public bool Result { get; set; }
    public int UncoveredSegments { get; set; }
}