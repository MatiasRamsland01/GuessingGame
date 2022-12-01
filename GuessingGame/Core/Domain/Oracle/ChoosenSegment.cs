using System.ComponentModel.DataAnnotations;

namespace GuessingGame.Core.Domain.Oracle;

public class ChoosenSegment
{
    public ChoosenSegment(int imageId, int segmentId)
    {
        ImageId = imageId;
        SegmentId = segmentId;
    }

    [Key]
    public Guid Id { get; set; }
    public int ImageId { get; set; }
    public int SegmentId { get; set; }
}