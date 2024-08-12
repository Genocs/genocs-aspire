using GenocsAspire.Multitenancy.Infrastructure.Multitenancy;

namespace GenocsAspire.Multitenancy.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(GNXTenantInfo tenant, CancellationToken cancellationToken);
}