using Microsoft.AspNetCore.Mvc;

namespace DemoBackend.Common.Results;

public static class ResultsExtensions
{
    // Extension method to easily map result to IActionResult
    public static IActionResult ToProblemDetailsResponse<T>(this Result<T> result,
        ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.Ok(result.Value);

        var firstError = result.Errors[0];
        var status = firstError.Code.ToStatusCode();
        return controller.Problem(statusCode: status, title: firstError.Description);
    }

    // Helper method to translate result error codes to http error codes
    private static int ToStatusCode(this ErrorCode code)
    {
        return code switch
        {
            ErrorCode.Validation => StatusCodes.Status400BadRequest,
            ErrorCode.NotFound => StatusCodes.Status404NotFound,
            ErrorCode.Conflict => StatusCodes.Status409Conflict,
            ErrorCode.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorCode.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}