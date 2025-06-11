using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeShare.Application.Contracts.Persistence;
using RecipeShare.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using RecipeShare.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace RecipeShare.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
       IConfiguration configuration)
        {
            services.AddDbContext<RecipeDatabaseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Server=localhost\\SQLEXPRESS01;Database=db_recipe;Trusted_Connection=True;MultipleActiveResultSets=true"));

                options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IDietaryTagRepository, DietaryTagRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();

            return services;
        }
    }
}
