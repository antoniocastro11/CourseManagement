namespace CourseManagementAPI.Exceptions;
public class EnrollmentException : Exception
{
    // poderia criar outros tipos de enrollment exception se eu quisesse retornar outros status codes com base no tipo do erro
    public EnrollmentException(string? message) : base(message){}
    public EnrollmentException(string? message, Exception? innerException) : base(message, innerException) {}
}