using System.Net;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Shared.Common.ApiResponse;
using FluentValidation;
namespace StudentManagement.Application.Api.Middlewares

   
{
    public class GlobalExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"[error] : {mess}", ex.Message);
                await HandleExceptionAsync(context, ex);

            }

        }

        public static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            int statusCode = ex switch { 
            NotFoundException => (int) HttpStatusCode.NotFound,
            BadRequestException => (int) HttpStatusCode.BadRequest,
                ValidationException => (int)HttpStatusCode.BadRequest,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                _ => (int) HttpStatusCode.InternalServerError
            
            };
            context.Response.StatusCode= statusCode;

            var message = ex switch {
                NotFoundException => ex.Message,
                BadRequestException => ex.Message,
                ValidationException valEx =>                              
                    string.Join(" | ", valEx.Errors.Select(e => e.ErrorMessage)),
                UnauthorizedException => ex.Message,  
                _ => " da xay ra loi he thong vui long thu lai sau"


            };


            var errorResponse = ApiResponse<object>.Fail(message);
            await context.Response.WriteAsJsonAsync(errorResponse);



        }
    }
}
