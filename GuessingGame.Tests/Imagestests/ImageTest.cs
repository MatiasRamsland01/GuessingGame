using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using GuessingGame.Core.Domain.Images.Pipelines;
using MediatR;
using GuessingGame.Core.Domain.Oracle;

namespace GuessingGame.Tests.Imagestests;

[Collection("Tests")]
public class ImageTests : IClassFixture<FactoryTest>
{
    private readonly WebApplicationFactory<Program> factory;

    public ImageTests(FactoryTest factory)
    {
        this.factory = factory;
    }

    [Fact]
    public void TestGetFullImageWithId()
    {
        using (var scope = factory.Services.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = mediator.Send(new GetFullImageWithImageIdBase64.Request(1));
            Assert.IsType<Task<List<String>>>(result);
        }
    }

    [Fact]
    public void TestGetImagesWithId()
    {
        using (var scope = factory.Services.CreateScope())
        {
            var userId = Guid.NewGuid();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var segmentlist = new List<ChoosenSegment> {new ChoosenSegment(1, 2) {
                    Id = userId}, new ChoosenSegment(1, 3) {
                    Id = userId
                    }};
            var result = mediator.Send(new GetImagesWithId.Request((segmentlist)));
            Assert.IsType<Task<List<String>>>(result);
        }
    }
}
