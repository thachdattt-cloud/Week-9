// Api/Module/ServiceRegistrationModule.cs
using tuan3.Repository.Interfaces;
using tuan3.Repository.Implementations;
using tuan3.Services.Interfaces;
using tuan3.Services.Implementations;

namespace tuan3.Api.Module
{
    public static class ServiceRegistrationModule
    {
        public static IServiceCollection AddStudentModule(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentService, StudentService>();

            return services;
        }
    }
}