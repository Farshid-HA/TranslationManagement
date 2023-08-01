using DataAccess.Databases;
using DataAccess.MapperProfile;
using DataAccess.Repositories;
using Domain.IServices;
using Domain.Repositories;
using External.ThirdParty.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TranslationManagement.Domain.IServices;
using TranslationManagement.Services.Services;

namespace Configuration
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ITranslationJobServices, TranslationJobServices>();
            services.AddTransient<ITranslatorManagementService, TranslatorManagementService>();
            services.AddTransient<INotificationService, UnreliableNotificationService>();
            services.AddTransient<IReliableNotificationService, ReliableNotificationService>();
            
        }
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        public static void AddDatabaseContext(this IServiceCollection service)
        {
            service.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source=TranslationAppDatabase.db");
            });
        }
        public static void RegisterMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TranslationJobProfile));
            services.AddAutoMapper(typeof(TranslatorModelProfile));
        }
    }
}
