﻿using Genocs.MultitenancyAspire.Infrastructure.Auth;
using Genocs.MultitenancyAspire.Infrastructure.Common;
using Genocs.MultitenancyAspire.Infrastructure.Multitenancy;
using Genocs.MultitenancyAspire.Shared.Multitenancy;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Genocs.MultitenancyAspire.Infrastructure.BackgroundJobs;

public class GNXJobActivator : JobActivator
{
    private readonly IServiceScopeFactory _scopeFactory;

    public GNXJobActivator(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

    public override JobActivatorScope BeginScope(PerformContext context)
        => new Scope(context, _scopeFactory.CreateScope());

    private class Scope : JobActivatorScope, IServiceProvider
    {
        private readonly PerformContext _context;
        private readonly IServiceScope _scope;

        public Scope(PerformContext context, IServiceScope scope)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            ReceiveParameters();
        }

        private void ReceiveParameters()
        {
            var tenantInfo = _context.GetJobParameter<GNXTenantInfo>(MultitenancyConstants.TenantIdName);
            if (tenantInfo is not null)
            {
                // TODO: Log the issue

                // _scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
                //    .MultiTenantContext = new MultiTenantContext<GNXTenantInfo>
                //    {
                //        TenantInfo = tenantInfo
                //    };
            }

            string userId = _context.GetJobParameter<string>(QueryStringKeys.UserId);
            if (!string.IsNullOrEmpty(userId))
            {
                _scope.ServiceProvider.GetRequiredService<ICurrentUserInitializer>()
                    .SetCurrentUserId(userId);
            }
        }

        public override object Resolve(Type type)
            => ActivatorUtilities.GetServiceOrCreateInstance(this, type);

        object? IServiceProvider.GetService(Type serviceType)
            => serviceType == typeof(PerformContext)
                ? _context
                : _scope.ServiceProvider.GetService(serviceType);
    }
}