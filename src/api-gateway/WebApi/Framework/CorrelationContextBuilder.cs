using System.Security.Claims;

namespace GenocsAspire.APIGateway.Framework;

internal sealed class CorrelationContextBuilder : ICorrelationContextBuilder
{
    public CorrelationContext Build(
                                    HttpContext context,
                                    string correlationId,
                                    string spanContext,
                                    string? name = null,
                                    string? resourceId = null)
        => new CorrelationContext
        {
            CorrelationId = correlationId,
            Name = name ?? string.Empty,
            ResourceId = resourceId ?? string.Empty,
            SpanContext = spanContext ?? string.Empty,
            TraceId = context.TraceIdentifier,
            ConnectionId = context.Connection.Id,
            CreatedAt = DateTime.UtcNow,
            User = new CorrelationContext.UserContext
            {
                Id = context.User.Identity.Name,
                IsAuthenticated = context.User.Identity.IsAuthenticated,
                Role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                Claims = context.User.Claims.ToDictionary(c => c.Type, c => c.Value)
            }
        };
}