using Finbuckle.MultiTenant.Abstractions;
using GenocsAspire.Multitenancy.Application.Common.Events;
using GenocsAspire.Multitenancy.Application.Common.Interfaces;
using GenocsAspire.Multitenancy.Domain.Catalog;
using GenocsAspire.Multitenancy.Infrastructure.Multitenancy;
using GenocsAspire.Multitenancy.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GenocsAspire.Multitenancy.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(IMultiTenantContextAccessor<GNXTenantInfo> multiTenantContextAccessor, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(multiTenantContextAccessor, options, currentUser, serializer, dbSettings, events)
    {
    }
    public ApplicationDbContext(IMultiTenantContext<GNXTenantInfo> multiTenantContextAccessor, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(multiTenantContextAccessor, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}