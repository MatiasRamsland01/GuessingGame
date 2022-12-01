namespace GuessingGame.Core.Domain.Images.DTO;

public class ImageDTO
{
    public ImageDTO(int imageId, string imageName, int segmentId, byte[] segment, string category)
    {
        ImageId = imageId;
        ImageName = imageName;
        SegmentId = segmentId;
        Segment = segment;
        Category = category;
    }
    public int ImageId { get; set; }
    public string ImageName { get; set; }
    public int SegmentId { get; set; }
    public byte[] Segment { get; set; }
    public string Category { get; set; }
}