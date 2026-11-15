using BLL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace projectApi.Filters
{
    public class GlobalExceptionFilter: IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            _logger.LogError(ex, "Unhandled exception at {Path}", context.HttpContext.Request.Path);

            int status = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                BusinessRuleException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            string message = status == 500
                ? "An unexpected error occurred. Please try again later."
                : ex.Message;

            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = status,
                Title = message
            })
            {
                StatusCode = status
            };

            context.ExceptionHandled = true;
        }
    }
}
