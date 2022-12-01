namespace GuessingGame.Core.Domain.History
{
    public class History
    {
        public History(int gameStateId, int imageId, int uncoveredSegments, string username)
        {
            GameStateId = gameStateId;
            ImageId = imageId;
            UncoveredSegments = uncoveredSegments;
            Date = DateTime.Now.ToLongDateString();
            Username = username;
        }
        public int Id { get; set; }
        public int GameStateId { get; set; }
        public int ImageId { get; set; }
        //This will be the score
        public int UncoveredSegments { get; set; }
        public string Date { get; set; }
        public string Username { get; set; }
    }
}

