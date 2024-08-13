using Genocs.WebApi.Exceptions;
using System.Net;

namespace GenocsAspire.OrdersApiService;

public class ExceptionToResponseMapper : IExceptionToResponseMapper
{
    public ExceptionResponse Map(Exception exception)
        => new(new { code = "error", message = exception.Message }, HttpStatusCode.BadRequest);
}