using System.ComponentModel.DataAnnotations;

namespace GuessingGame.Core.Domain.Images;

public class Image
{
    public Image(int imageId, string imageName, int segmentId, byte[] segment, string category)
    {
        ImageId = imageId;
        ImageName = imageName;
        SegmentId = segmentId;
        Segment = segment;
        Category = category;
    }

    [Key]
    public Guid Id { get; protected set; }
    public int ImageId { get; set; }
    public string ImageName { get; set; }
    public int SegmentId { get; set; }
    public byte[] Segment { get; set; }
    public string Category { get; set; }
    public int PriorityScoreForBetterSegment { get; set; }

}