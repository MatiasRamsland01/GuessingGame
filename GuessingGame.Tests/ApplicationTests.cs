using GuessingGame.Core.Domain.Images.Services;
using GuessingGame.Core.Domain.Oracle.Services;
using GuessingGame.Core.Domain.User.Services;
using GuessingGame.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GuessingGame.Tests
{
    public class FactoryTest : WebApplicationFactory<Program>
    {
        public FactoryTest()
        {
            if (Directory.GetCurrentDirectory().Contains("GuessingGame.Tests"))
                Directory.SetCurrentDirectory("../../../../GuessingGame");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
               {
                   var g = Guid.NewGuid().ToString();
                   var opt = services.FirstOrDefault(a => a.ServiceType == typeof(DbContextOptions<GuessingGameDbContext>));
                   if (opt != null)
                       services.Remove(opt);
                   services.AddDbContext<GuessingGameDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting" + g);
                    });

               });
        }
    }

    [Collection("Tests")]
    public class ApplicationTests : IClassFixture<FactoryTest>
    {
        private readonly WebApplicationFactory<Program> factory;

        public ApplicationTests(FactoryTest factoryTest)
        {
            this.factory = factoryTest;
        }

        [Fact]
        public void IOracleServiceTest()
        {
            using var scope = factory.Services.CreateScope();
            var oracleService = scope.ServiceProvider.GetService<IOracleService>();

            Assert.NotNull(oracleService);
        }

        [Fact]
        public void IIdentityServiceTest()
        {
            using var scope = factory.Server.Services.CreateScope();
            var identityService = scope.ServiceProvider.GetService<IIdentityService>();

            Assert.NotNull(identityService);
        }

        [Fact]
        public void IImageServiceTest()
        {

            using var scope = factory.Server.Services.CreateScope();
            var identityService = scope.ServiceProvider.GetService<IImageService>();

            Assert.NotNull(identityService);
        }

        [Fact]
        public void TestRandomImageID()
        {
            using var scope = factory.Services.CreateScope();
            var oracleService = scope.ServiceProvider.GetService<IOracleService>();

            if (oracleService != null)
            {
                Assert.NotNull(oracleService.GetRandomImageId("Other"));
            }

        }

    }
}