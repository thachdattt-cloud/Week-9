// Api/Module/ServiceRegistrationModule.cs

using FluentValidation;
using MediatR;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Infrastructure.Context;
using StudentManagement.Infrastructure.Repository.Implementations;
using StudentManagement.Application.Behaviors;
using StudentManagement.Application.Features.Students.Commands.CreateStudent;

namespace StudentManagement.Application.Api.Module
{
    public static class ServiceRegistrationModule
    {
        public static IServiceCollection AddStudentModule(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
           
            services.AddScoped<DapperContext>();
            services.AddValidatorsFromAssembly(typeof(CreateStudentCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
      
    }
}