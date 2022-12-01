namespace GuessingGame.Core.Domain.History.Services;

public class HistoryService : IHistoryService
{
    public int CalculateScore(int totalSegments, int uncoveredSegments, bool userWon)
    {
        if (!userWon)
        {
            return 0;
        }
        //calculate score relative to total amount of segments in image
        double differ = ((double)uncoveredSegments / totalSegments);
        double point = differ * 100;
        var points = Convert.ToInt32(point);
        return points;

    }
}