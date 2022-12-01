using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MediatR;
using GuessingGame.Core.Domain.User.Services;
using GuessingGame.Core.Domain.Oracle.Services;
using GuessingGame.Infrastructure.Data;
using GuessingGame.Core.Domain.Images.Services;
using GuessingGame.Core.Domain.History.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60); // We're keeping this low to facilitate testing. Would normally be higher. Default is 20 minutes
    options.Cookie.IsEssential = true;              // Otherwise we need cookie approval
});
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IOracleService, OracleService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddTransient<IHistoryService, HistoryService>();

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("GuessingGameDbContext") ?? throw new InvalidOperationException("Connection string 'GuessingGameDbContext' not found.");
builder.Services.AddDbContext<GuessingGame.Infrastructure.Data.GuessingGameDbContext>(options =>
{
    options.UseSqlite(connectionString);
});


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<GuessingGame.Infrastructure.Data.GuessingGameDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 0;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();
//app.UseExceptionHandler("/Error");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GuessingGame.Infrastructure.Data.GuessingGameDbContext>();
    db.Database.EnsureCreated();
    if (!db.Images.Any())
    {
        ImageData.Init();
        HistoryData.Init();
        foreach (var item in ImageData.images)
        {
            db.Images.Add(item);
        }
        foreach (var item in HistoryData.histories)
        {
            db.History.Add(item);
        }
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); ;
app.UseAuthorization();

app.MapRazorPages();

app.Run();

// to be able to perform tests on the code
public partial class Program { }