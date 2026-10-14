using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Exceptions.Handlers;
public class EnrollmentExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if(exception is not EnrollmentException) return  false;
        ProblemDetails problemDetails = new()
        {
            Title = "Não foi possível criar a matrícula",
            Status = StatusCodes.Status400BadRequest,
            Detail = exception.Message
        };
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}