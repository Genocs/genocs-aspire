using Finbuckle.MultiTenant;
using GenocsAspire.Multitenancy.Infrastructure.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenocsAspire.Multitenancy.Infrastructure.Persistence.Configuration;

public class AuditTrailConfig : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder) =>
        builder
            .ToTable("AuditTrails", SchemaNames.Auditing)
            .IsMultiTenant();
}