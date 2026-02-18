using Homesplash.Data;
using Homesplash.Logos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<LinkTileContext>(opts =>
    opts.UseSqlite(builder.Configuration.GetConnectionString("LinkTileDb")));

builder.Services.AddSingleton<ILogoQueue, LogoQueue>();
builder.Services.AddScoped<ILogoService, LogoService>();
builder.Services.AddHostedService<LogoWorker>();

builder.AddLinkTileDb();

builder.Services.AddCors(options => {
    options.AddPolicy("OpenPolicy", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("OpenPolicy");
app.MapControllers();

app.MigrateDb();

app.Run();