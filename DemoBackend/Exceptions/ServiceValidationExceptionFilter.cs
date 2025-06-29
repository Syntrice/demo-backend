using DemoBackend.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoBackend.Exceptions;

public sealed class ServiceValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ServiceValidationException ex) return;

        context.Result = new BadRequestObjectResult(
            new ErrorResponseModel { Errors = [ex.Message] });

        context.ExceptionHandled = true;
    }
}
