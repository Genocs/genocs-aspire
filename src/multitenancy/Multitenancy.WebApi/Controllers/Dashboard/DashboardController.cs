using Genocs.MultitenancyAspire.Application.Dashboard;

namespace Genocs.MultitenancyAspire.WebApi.Controllers.Dashboard;

public class DashboardController : VersionedApiController
{
    [HttpGet]
    [MustHavePermission(GNXAction.View, GNXResource.Dashboard)]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public Task<StatsDto> GetAsync()
    {
        return Mediator.Send(new GetStatsRequest());
    }
}