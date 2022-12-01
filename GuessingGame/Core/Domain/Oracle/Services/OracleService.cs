using GuessingGame.Core.Domain.Images.Pipelines;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.CodeAnalysis;
using System.Text.Json;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Core.Domain.Images.DTO;



namespace GuessingGame.Core.Domain.Oracle.Services
{
    public class OracleService : IOracleService
    {
        private readonly IMediator _mediator;
        private readonly GuessingGameDbContext _db;

        public OracleService(IMediator mediator, GuessingGameDbContext db)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _db = db;
        }

        public async Task<int> GetRandomPuzzlePieceId(GameState gameState, int imageId)
        {
            if (gameState is null)
            {
                throw new ArgumentNullException("There is no gamestate!");
            }

            var choosenSegments = gameState.ChoosenSegments;

            var chosenSegmentIdList = new HashSet<int>();

            foreach (ChoosenSegment chosensegment in choosenSegments)
            {
                chosenSegmentIdList.Add(chosensegment.SegmentId);
            }

            var segmentIds = await _mediator.Send(new GetSegmentIdsInImage.Request(imageId));

            var segmentIdsStillAvailable = Enumerable.Range(0, segmentIds.Count()).Where(i => !chosenSegmentIdList.Contains(i));


            var rand = new System.Random();

            int index = rand.Next(segmentIdsStillAvailable.Count());

            return segmentIdsStillAvailable.ElementAt(index);
        }
        public async Task<int> GetRandomImageId(string category)
        {
            var ids = await _mediator.Send(new GetImageIds.Request(category));
            Random rnd = new Random();
            int index = rnd.Next(ids.Count);
            int id = ids[index];
            return id;
        }

        public async Task<GameState> ProposeBetterSegments(JsonElement json, Guid userGuid)
        {
            BetterSegments? betterSegments = JsonSerializer.Deserialize<BetterSegments>(json.GetRawText());

            if (betterSegments is null)
            {
                throw new NullReferenceException("betterSegments was null!");
            }

            int x = betterSegments.xCoord;
            int y = betterSegments.yCoord;
            int imageId = betterSegments.ImageId;

            //get all segments with corresponding segment id
            List<ImageDTO> imageDTOs = await _mediator.Send(new GetSegments.Request(imageId));
            var state = await _db.GameStates
                .Where(s => s.UserGuid == userGuid)
                .Include(s => s.ChoosenSegments)
                .Include(f => f.ProposedSegmentIds)
                .FirstOrDefaultAsync() ?? throw new Exception($"Could not retrieve gamestate with userid: {userGuid}");


            // finds which segment was clicked/proposed as better
            foreach (var imageDTO in imageDTOs)
            {
                using SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(imageDTO.Segment);
                var color = image[x, y];

                var opacity = color.A;
                if (opacity == 255)
                {
                    //the image is not transparent here!
                    var exists = state.ProposedSegmentIds.FirstOrDefault(f => f.SegmentId == imageDTO.SegmentId);
                    if (exists is not null)
                    {
                        break;
                    }
                    state.AddPropsedId(imageDTO.SegmentId, x, y);
                    await _mediator.Send(new AddPriorityScoreForBetterSegment.Request(imageDTO.SegmentId, imageId, 1));
                    state.RemoveChoosenSegment(imageDTO.SegmentId);
                    await _db.SaveChangesAsync();
                }
            }
            return state;
        }
    }
}
