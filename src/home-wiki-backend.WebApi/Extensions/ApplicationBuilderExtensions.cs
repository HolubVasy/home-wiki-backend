using home_wiki_backend.Contracts;
using home_wiki_backend.DAL.Common.Data;
using Microsoft.EntityFrameworkCore;


/// <summary>
/// Provides extension methods for IApplicationBuilder.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Applies any pending migrations for the context to the database 
    /// and seeds initial data.
    /// </summary>
    /// <param name="app">The IApplicationBuilder instance.</param>
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
