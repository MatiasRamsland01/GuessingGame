namespace GuessingGame.Core.Domain.Oracle;

public class GameState
{
    public int Id { get; set; }
    public Guid UserGuid { get; set; }
    public bool UserCanGuess { get; set; } = true;
    public bool GameFinished { get; set; } = false;
    public bool GameWon { get; set; } = false;
    private int _usedGuesses = 0;
    public int UsedGuesses
    {
        get
        {
            return _usedGuesses;
        }
        set
        {
            _usedGuesses = value;
            if (_usedGuesses > 2)
            {
                UserCanGuess = false;
            }
        }

    }
    public readonly List<BetterSegments> _ProposedSegmentIds = new();

    public IEnumerable<BetterSegments> ProposedSegmentIds => _ProposedSegmentIds.AsReadOnly();

    public string CorrectGuess { get; set; } = "";
    public int ImageId { get; set; }
    public int segmentsInImage { get; set; }
    public int ShownSegmentsNumber { get; set; }

    public readonly List<ChoosenSegment> _ChoosenSegments = new();
    public IEnumerable<ChoosenSegment> ChoosenSegments => _ChoosenSegments.AsReadOnly();

    public void AddPropsedId(int segmentId, int xCoord, int yCoord)
    {
        var proposedSegmentToAdd = new BetterSegments();
        proposedSegmentToAdd.xCoord = xCoord;
        proposedSegmentToAdd.yCoord = yCoord;
        proposedSegmentToAdd.ImageId = ImageId;
        proposedSegmentToAdd.SegmentId = segmentId;
        _ProposedSegmentIds.Add(proposedSegmentToAdd);
    }
    public void AddChoosenSegment(int imageId, int segmentId)
    {
        var choosenSegmentToAdd = new ChoosenSegment(imageId, segmentId);
        _ChoosenSegments.Add(choosenSegmentToAdd);
    }
    public void RemoveChoosenSegment(int segmentId)
    {
        _ChoosenSegments.RemoveAll(i => i.SegmentId == segmentId);
    }
    public void ClearChoosenSegments()
    {
        _ChoosenSegments.Clear();
    }
}