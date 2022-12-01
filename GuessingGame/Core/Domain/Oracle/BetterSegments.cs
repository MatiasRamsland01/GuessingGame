namespace GuessingGame.Core.Domain.Oracle;

public class BetterSegments
{
    public BetterSegments()
    { }

    public Guid Id { get; protected set; }
    public int xCoord { get; set; }
    public int yCoord { get; set; }
    public int ImageId { get; set; }
    public int SegmentId { get; set; }

}