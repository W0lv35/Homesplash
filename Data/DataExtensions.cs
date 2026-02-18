using Homesplash.Models;
using Microsoft.EntityFrameworkCore;

namespace Homesplash.Data;

internal static class DataExtensions {
    public static void MigrateDb(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LinkTileContext>();
        context.Database.Migrate();
    }

    public static void AddLinkTileDb(this WebApplicationBuilder builder) {
        var connectionString = builder.Configuration.GetConnectionString("LinkTileDb");
        builder.Services.AddSqlite<LinkTileContext>(connectionString, null, options => {
            options.UseSeeding((context, _) => {
                if (!context.Set<Category>().Any()) {
                    context.Set<Category>().AddRange(
                        new Category { Name = "Main" },
                        new Category { Name = "Hytale Modding" },
                        new Category { Name = "Linux" },
                        new Category { Name = "C#" },
                        new Category { Name = "Unity" }
                    );
                    context.SaveChanges();
                }
            });
        });
    }
}