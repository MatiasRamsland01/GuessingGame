namespace GuessingGame.Core.Domain.Oracle;

public class SegmentIdAndImg
{
    public SegmentIdAndImg(int segmentId, byte[] imageSharpImg)
    {
        SegmentId = segmentId;
        ImageSharpImg = imageSharpImg;

    }
    public int SegmentId { get; set; }
    public byte[] ImageSharpImg { get; set; }
}