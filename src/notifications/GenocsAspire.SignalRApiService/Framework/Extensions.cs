using Genocs.Core.Builders;
using GenocsAspire.SignalRApiService.Configurations;

namespace GenocsAspire.SignalRApiService.Framework;

public static class Extensions
{
    public static string ToUserGroup(this Guid userId)
        => $"users:{userId}";

    public static IGenocsBuilder AddSignalR(this IGenocsBuilder builder)
    {
        var options = builder.Configuration.GetOptions<SignalROptions>("signalr");

        if (options is not null)
        {
            builder.Services.AddSingleton(options);
        }

        return builder;
    }

    public static IGenocsBuilder UseSignalR(this IGenocsBuilder builder)
    {
        var options = builder.GetOptions<SignalROptions>("signalr");

        if (options is not null)
        {
            builder.Services.AddSingleton(options);
        }

        return builder;
    }
}