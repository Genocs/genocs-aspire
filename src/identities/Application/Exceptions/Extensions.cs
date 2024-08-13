using Genocs.Core.Extensions;

namespace GenocsAspire.Identities.Application.Exceptions;

internal static class Extensions
{
    public static string GetExceptionCode(this Exception exception)
        => exception.GetType().Name.Underscore().Replace("_exception", string.Empty);
}