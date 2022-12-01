using System.Text.Json;

namespace GuessingGame.Core.Domain.Oracle.Services
{
    public interface IOracleService
    {
        Task<int> GetRandomPuzzlePieceId(GameState gamestate, int imageId);
        Task<int> GetRandomImageId(string category);
        Task<GameState> ProposeBetterSegments(JsonElement json, Guid userGuid);
    }
}