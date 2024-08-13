using Genocs.Core.Builders;
using Genocs.Logging;
using Genocs.Secrets.Vault;
using Genocs.WebApi;
using Genocs.WebApi.CQRS;
using GenocsAspire.Identities.Application;
using GenocsAspire.Identities.Application.Commands;
using GenocsAspire.Identities.Application.DTO;
using GenocsAspire.Identities.Application.Queries;
using GenocsAspire.Identities.Application.Services;
using Serilog;

StaticLogger.EnsureInitialized();

var builder = WebApplication.CreateBuilder(args);

builder.Host
        .UseLogging()
        .UseVault();

var services = builder.Services;

services.AddGenocs(builder.Configuration)
        .AddWebApi()
        .AddCore()
        .Build();

var app = builder.Build();

app.UseCore();
app.UseDispatcherEndpoints(endpoints => endpoints
                            .Get(string.Empty, ctx => ctx.GetAppName())
                            .Post<SignIn>("sign-in", afterDispatch: (cmd, ctx) =>
                            {
                                var auth = ctx.RequestServices.GetRequiredService<ITokenStorage>().Get(cmd.Id);
                                return ctx.Response.WriteJsonAsync(auth);
                            })
                            .Post<CreateUser>("sign-up", afterDispatch: (cmd, ctx) =>
                            {
                                ctx.Response.Headers.Append("user-id", cmd.UserId.ToString());
                                return Task.CompletedTask;
                            })
                            .Post<RevokeAccessToken>("access-tokens/revoke", auth: true, roles: "admin")
                            .Post<UseRefreshToken>("refresh-tokens/use", afterDispatch: (cmd, ctx) =>
                            {
                                var auth = ctx.RequestServices.GetRequiredService<ITokenStorage>().Get(cmd.Id);
                                return ctx.Response.WriteJsonAsync(auth);
                            })
                            .Post<RevokeRefreshToken>("refresh-tokens/revoke", auth: true, roles: "admin")
                            .Get<GetUser, UserDetailsDto>("users/{userId:guid}", auth: true)
                            .Get<BrowseUsers, PagedDto<UserDto>>("users", auth: true)
                            .Put<LockUser>("users/{userId:guid}/lock", auth: true, roles: "admin")
                            .Put<UnlockUser>("users/{userId:guid}/unlock", auth: true, roles: "admin"));

app.Run();

Log.CloseAndFlush();