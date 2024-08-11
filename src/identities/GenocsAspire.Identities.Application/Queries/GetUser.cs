using Genocs.Core.CQRS.Queries;
using GenocsAspire.Identities.Application.DTO;

namespace GenocsAspire.Identities.Application.Queries;

public class GetUser : IQuery<UserDetailsDto>
{
    public Guid UserId { get; set; }
}