using home_wiki_backend.Contracts;
using home_wiki_backend.DAL.Data;
using Microsoft.EntityFrameworkCore;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrationsAndSeed(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbWikiContext>();
        db.Database.Migrate();

        var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
        if (!seeder.AlreadySeeded())
        {
            seeder.Seed();
        }
    }
}