using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using GenocsAspire.Multitenancy.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GenocsAspire.Multitenancy.Infrastructure.Multitenancy;

public class TenantDbContext : EFCoreStoreDbContext<GNXTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GNXTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}