namespace GuessingGame.Core.Domain.History.Services;

public interface IHistoryService
{
    int CalculateScore(int totalSegments, int uncoveredSegments, bool userWon);
}

