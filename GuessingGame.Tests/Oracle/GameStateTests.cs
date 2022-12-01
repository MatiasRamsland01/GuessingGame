using GuessingGame.Core.Domain.Oracle;
using GuessingGame.Core.Domain.Oracle.Pipelines;
using GuessingGame.Core.Domain.User.Pipelines;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GuessingGame.Tests.Oracle;

[Collection("Tests")]
public class GameStateTests : IClassFixture<FactoryTest>
{
    private readonly WebApplicationFactory<Program> factory;

    public GameStateTests(FactoryTest factoryTest)
    {
        this.factory = factoryTest;
    }

    [Fact]
    public Task AddChoosenSegmentTest()
    {
        var gamestate = new GameState();
        gamestate.AddChoosenSegment(6, 5);

        Assert.Single(gamestate.ChoosenSegments);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetGameStateTest()
    {
        var userId = Guid.NewGuid();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            db.GameStates.Add(new GameState()
            {
                UserGuid = userId,
                CorrectGuess = "",
            });
            db.SaveChanges();
        }
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new GetGameState.Request(userId));

            Assert.Equal(result.UserGuid, userId);
        }

    }

    [Fact]
    public async void CheckDoneGuessingTests()
    {
        var userId = Guid.NewGuid();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            db.GameStates.Add(new GameState()
            {
                UserGuid = userId,
                CorrectGuess = "",
                UserCanGuess = false,
            });
            db.SaveChanges();
        }
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new CheckDoneGuessing.Request(userId));

            Assert.False(result);
        }
    }

    [Fact]
    public async Task VerifyGuessWhenCorrectTest()
    {
        var userId = Guid.NewGuid();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            db.GameStates.Add(new GameState()
            {
                UserGuid = userId,
                CorrectGuess = "Dog",
            });
            db.SaveChanges();
        }
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new VerifyGuess.Request(userId, "dog"));

            Assert.True(result);
        }
    }
    [Fact]
    public async Task VerifyGuessWhenWrongTest()
    {
        var userId = Guid.NewGuid();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            db.GameStates.Add(new GameState()
            {
                UserGuid = userId,
                CorrectGuess = "Dog",
            });
            db.SaveChanges();
        }
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new VerifyGuess.Request(userId, "cat"));

            Assert.False(result);
        }
    }

    [Fact]
    public async Task RetrieveImageSliceIdTest()
    {
        var userId = Guid.NewGuid();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            db.GameStates.Add(new GameState()
            {
                UserGuid = userId,
                CorrectGuess = "",
            });
            db.SaveChanges();
        }
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var gameState = await mediator.Send(new GetGameState.Request(userId));
            gameState.AddChoosenSegment(1, 1);

            var result = await mediator.Send(new RetrieveImageSegmentsId.Request(userId));

            foreach (var id in result)
            {
                Assert.Equal(1, id.ImageId);
                Assert.Equal(1, id.SegmentId);
            }
            Assert.Single(result);
        }
    }

    [Fact]
    public async Task NewImageSegmentInGamestateTestAsync()
    {
        var userId = Guid.NewGuid();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            db.GameStates.Add(new GameState()
            {
                UserGuid = userId,
                CorrectGuess = "",
                ImageId = 2,
            });
            db.SaveChanges();
        }
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var NewSegmentInGameState = await mediator.Send(new NewImageSegmentInGameState.Request(userId));

            var result = await mediator.Send(new RetrieveImageSegmentsId.Request(userId));

            foreach (var id in result)
            {
                Assert.Equal(2, id.ImageId);
                Assert.NotEqual(0, id.SegmentId);
            }
            Assert.Single(result);
        }
    }
}