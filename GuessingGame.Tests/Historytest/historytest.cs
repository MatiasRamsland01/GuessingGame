using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using GuessingGame.Core.Domain.History.Pipelines;
using MediatR;
using GuessingGame.Core.Domain.History;
using GuessingGame.Core.Domain.History.Services;
using GuessingGame.Core.Domain.Oracle;
using GuessingGame.Core.Domain.Oracle.Pipelines;
using GuessingGame.Core.Domain.User.Pipelines;
using GuessingGame.Infrastructure.Data;


namespace GuessingGame.Tests.Historytest;
[Collection("Tests")]

public class HistoryTests : IClassFixture<FactoryTest>
{
    private readonly WebApplicationFactory<Program> factory;

    public HistoryTests(FactoryTest factoryTest)
    {
        this.factory = factoryTest;
    }

    [Fact]
    public void TestGetLeaderBoard()
    {
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
        }
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = mediator.Send(new GetLeaderboard.Request());
            Assert.IsType<Task<List<History>>>(result);
        }
    }


    [Fact]
    public void TestGetRecentGamesWithUserId()
    {
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
        }

        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = mediator.Send(new GetRecentGamesWithUserId.Request("test"));
            Assert.IsType<Task<List<History>>>(result);
        }
    }


    [Theory]
    [InlineData(100, 50, false, 0)]
    public void TestCalculateScore(int totalSegments, int uncoveredSegments, bool gameWon, int expectedScore)
    {
        using (var scope = factory.Services.CreateScope())
        {
            var historyService = scope.ServiceProvider.GetRequiredService<IHistoryService>();
            var score = historyService.CalculateScore(totalSegments, uncoveredSegments, gameWon);
            Assert.Equal(expectedScore, score);
        }
    }
}
