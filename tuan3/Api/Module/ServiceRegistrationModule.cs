// Api/Module/ServiceRegistrationModule.cs

using FluentValidation;
using MediatR;
using StudentManagement.Application.Behaviors;
using StudentManagement.Application.Features.Students.Commands.CreateStudent;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Infrastructure.Context;
using StudentManagement.Infrastructure.Implementations.Repositories;
using StudentManagement.Infrastructure.Repository.Implementations;
using StudentManagement.Infrastructure.Services;

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
            services.AddAutoMapper(typeof(CreateStudentCommand).Assembly);
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }
      
    }
}