using GuessingGame.Core.Domain.Images.Pipelines;
using GuessingGame.Core.Domain.Images.Services;
using GuessingGame.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GuessingGame.Tests.Imagestests;

[Collection("Tests")]
public class ImageServiceTest : IClassFixture<FactoryTest>
{
    private readonly WebApplicationFactory<Program> factory;

    public ImageServiceTest(FactoryTest factory)
    {
        this.factory = factory;
    }

    [Fact]
    public void SliceImageAutomaticTest()
    {
        var stream = File.OpenRead("../GuessingGame.Tests/dog.jpg");

        var file = new FormFile(stream, 0, stream.Length, "dog", Path.GetFileName(stream.Name));


        using (var scope = factory.Services.CreateScope())
        {
            var oracleService = scope.ServiceProvider.GetService<IImageService>();

            if (oracleService != null)
            {
                var dividedImage = oracleService.SliceImageAutomatic(file);
                Assert.NotEmpty(dividedImage);
            }
        }
    }

    [Fact]
    public async void AutomaticallySliceImageTest()
    {
        var stream = File.OpenRead("../GuessingGame.Tests/dog.jpg");
        var file = new FormFile(stream, 0, stream.Length, "dog", Path.GetFileName(stream.Name));

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GuessingGameDbContext>();
            var antImageBefore = db.Images.Select(i => i.ImageId).Distinct().ToList();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new AutomaticallySliceImage.Request(file, "dog", "animal"));

            var antImageAfter = db.Images.Select(k => k.ImageId).Distinct().ToList();

            var listImageSegment = db.Images.Where(i => i.ImageName == "dog").Select(j => j.ImageId).ToList();

            Assert.NotEmpty(listImageSegment);
            Assert.Equal(antImageAfter.Count(), antImageBefore.Count() + 1);

        }

    }

}