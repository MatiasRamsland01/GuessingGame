using GuessingGame.Core.Domain.Images.Pipelines;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GuessingGame.Tests.Oracle;

[Collection("Tests")]
public class ImageTests : IClassFixture<FactoryTest>
{
    private readonly WebApplicationFactory<Program> factory;

    public ImageTests(FactoryTest factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task GetImageIdsTest()
    {
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new GetImageIds.Request("allcategories"));

            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            var ids = await db.Images.Select(i => i.ImageId).ToListAsync();

            var isUnique = ids.Distinct().Count();

            Assert.Equal(isUnique, result.Count());

        }
    }

    [Fact]
    public async Task GetSegmentsIdsTest()
    {
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new GetSegmentIdsInImage.Request(1));

            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();

            var ids = await db.Images.Where(j => j.ImageId == 1).Select(i => i.SegmentId).Distinct().ToListAsync();

            Assert.Equal(ids.Count(), result.Distinct().Count());

        }
    }

}
