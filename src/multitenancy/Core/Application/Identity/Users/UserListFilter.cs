using GenocsAspire.Multitenancy.Application.Common.Models;

namespace GenocsAspire.Multitenancy.Application.Identity.Users;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}