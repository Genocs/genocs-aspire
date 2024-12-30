﻿using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Genocs.MultitenancyAspire.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Genocs.MultitenancyAspire.Infrastructure.Multitenancy;

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