using GuessingGame.Core.Domain.Oracle;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Core.Domain.Images;
using GuessingGame.Core.Domain.History;

namespace GuessingGame.Infrastructure.Data;

public class GuessingGameDbContext : IdentityDbContext<IdentityUser>
{
    public GuessingGameDbContext(DbContextOptions<GuessingGameDbContext> options)
        : base(options)
    {
    }
    public DbSet<GameState> GameStates { get; set; } = null!;

    public DbSet<Image> Images { get; set; } = null!;

    public DbSet<History> History { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<BetterSegments>().Ignore(c => c.xCoord).Ignore(c => c.yCoord);
        builder.Ignore<IdentityRole>();
        builder.Ignore<IdentityUserToken<string>>();
        builder.Ignore<IdentityUserRole<string>>();
        builder.Ignore<IdentityUserLogin<string>>();
        builder.Ignore<IdentityRoleClaim<string>>();
        builder.Entity<IdentityUser>()
            .Ignore(c => c.AccessFailedCount)
            .Ignore(c => c.LockoutEnabled)
            .Ignore(c => c.TwoFactorEnabled)
            .Ignore(c => c.ConcurrencyStamp)
            .Ignore(c => c.LockoutEnd)
            .Ignore(c => c.EmailConfirmed)
            .Ignore(c => c.TwoFactorEnabled)
            .Ignore(c => c.LockoutEnd)
            .Ignore(c => c.AccessFailedCount)
            .Ignore(c => c.PhoneNumberConfirmed)
            .Ignore(c => c.NormalizedEmail)
            .Ignore(c => c.PhoneNumber)
            .Ignore(c => c.SecurityStamp);


    }
}

internal class ImageData
{
    public static List<Image> images = new();

    internal static void Init()
    {

        int Count = 0;
        var curdir = Environment.CurrentDirectory;
        string path = curdir + "/Infrastructure/Data/Scattered_Images";
        foreach (string dirFile in Directory.GetDirectories(path))
        {
            Count += 1;
            foreach (string fileName in Directory.GetFiles(dirFile))
            {
                string segmentnumber = System.IO.Path.GetFileNameWithoutExtension(fileName);
                string imagename = System.IO.Path.GetFileNameWithoutExtension(dirFile);
                var imagename_split = imagename.Split("_");

                // Covert segment file to byte array
                byte[] bytearray = File.ReadAllBytes(fileName);
                images.Add(new Image(Count, imagename_split[0], Int16.Parse(segmentnumber), bytearray, imagename_split[1]));
            }
        }

    }
}
internal class HistoryData
{
    public static List<History> histories = new();

    internal static void Init()
    {
        var lorem = new Bogus.DataSets.Lorem();
        var random = new Bogus.Randomizer();
        histories = Enumerable.Range(1, 30).Select(i =>
        {
            var item = new History(
                random.Int(999, 9999),
                random.Int(1000, 10000),
                random.Int(10, 50),
                lorem.Random.AlphaNumeric(8));
            return item;
        }).ToList();
    }
}