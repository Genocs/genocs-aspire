using Genocs.Core.CQRS.Queries;
using GenocsAspire.Identities.Application.DTO;

namespace GenocsAspire.Identities.Application.Queries;

public class BrowseUsers : PagedQueryBase, IQuery<PagedDto<UserDto>>
{
    public Guid? UserId { get; set; }
}