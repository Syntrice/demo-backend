using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackend.Common.Results;

public static class ValidationResultExtensions
{
    // Extension method to easily map result to IActionResult
    public static IActionResult ToProblemDetailsResponse(this ValidationResult result, ControllerBase controller)
    {
        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        var extensions = new Dictionary<string, object?>
        {
            ["errors"] = errors
        };

        return controller.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "One or more validation errors occurred.",
            extensions: extensions);
    }
}