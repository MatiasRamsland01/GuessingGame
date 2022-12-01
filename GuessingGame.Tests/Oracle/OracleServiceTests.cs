using GuessingGame.Core.Domain.Images.Pipelines;
using GuessingGame.Core.Domain.Oracle;
using GuessingGame.Core.Domain.Oracle.Services;
using GuessingGame.Core.Domain.User.Pipelines;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GuessingGame.Tests.Oracle;

[Collection("Tests")]
public class OracleServiceTests : IClassFixture<FactoryTest>
{
    private readonly WebApplicationFactory<Program> factory;

    public OracleServiceTests(FactoryTest factory)
    {
        this.factory = factory;
    }

    [Fact]
    public void TestRandomImageID()
    {
        using (var scope = factory.Services.CreateScope())
        {
            var oracleService = scope.ServiceProvider.GetService<IOracleService>();

            if (oracleService != null)
            {
                Assert.NotNull(oracleService.GetRandomImageId("allcategories"));

            }
        }


    }

    [Fact]
    public async void TestRandomSegmentID()
    {
        // Check how many segments an image has (X), and to the method X times to check that every id is unique
        var userId = Guid.NewGuid();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            db.GameStates.Add(new GameState()
            {
                UserGuid = userId,
                CorrectGuess = "",
                ImageId = 1,
            });
            db.SaveChanges();
        }

        using (var scope = factory.Services.CreateScope())
        {
            var oracleService = scope.ServiceProvider.GetService<IOracleService>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var gameState = await mediator.Send(new GetGameState.Request(userId));
            var segments = await mediator.Send(new GetSegmentIdsInImage.Request(gameState.ImageId));

            var NewSegments = new List<int>();


            if (oracleService == null)
            {
                throw new Exception("No oracleService!");

            }

            foreach (var segment in segments)
            {
                var id = await oracleService.GetRandomPuzzlePieceId(gameState, gameState.ImageId);
                gameState.AddChoosenSegment(gameState.ImageId, id);
                NewSegments.Add(id);
            }
            var isUnique = NewSegments.Distinct().Count();

            Assert.Equal(isUnique, NewSegments.Count());
        }
    }
}
