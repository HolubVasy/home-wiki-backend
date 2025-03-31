using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Services;
using home_wiki_backend.Contracts;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.DAL.Repositories;
using home_wiki_backend.Data.Seeders;

namespace home_wiki_backend.Extensions
{
    /// <summary>
    /// Extension methods for configuring dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the dependency injection for the application.
        /// </summary>
        /// <param name="services">The service collection to add the dependencies to.</param>
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<Article>, GenericRepository<Article>>();
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddScoped<IGenericRepository<Tag>, GenericRepository<Tag>>();

            services.AddScoped<ISeeder, InitialSeeder>();

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITagService, TagService>();
        }
    }
}
