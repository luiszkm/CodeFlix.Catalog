using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodeFlix.Catalog.Api.FiltersExceptions;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    public ApiGlobalExceptionFilter(IHostEnvironment env)
    {
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();
        var exception = context.Exception;

        if (_env.IsDevelopment())
        {
            details.Extensions.Add("StackTrace", exception.StackTrace);
        }

        if (exception is NotFoundException)
        {
            details.Title = "Not Found";
            details.Status = StatusCodes.Status404NotFound;
            details.Type = "Not Found";
            details.Detail = exception!.Message;

        }
        else if (exception is EntityValidationException)
        {
            details.Title = "One or more validation errors occurred.";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnProcessableEntity";
            details.Detail = exception!.Message;

        }
        else
        {
            details.Title = "An unexpected error occurred";
            details.Status = StatusCodes.Status500InternalServerError;
            details.Type = "UnexpectedError";
            details.Detail = exception.Message;
        }

        context.HttpContext.Response.StatusCode = (int)details.Status;
        context.Result = new ObjectResult(details);
    }
}
