using Genocs.BlazorAspire.Client.Components.Common;
using Genocs.BlazorAspire.Infrastructure.ApiClient;
using Genocs.BlazorAspire.Client.Shared;
using Genocs.BlazorAspire.Shared.MultiTenancy;
using Microsoft.AspNetCore.Components;

namespace Genocs.BlazorAspire.Client.Pages.Authentication;

public partial class ForgotPassword
{
    private readonly ForgotPasswordRequest _forgotPasswordRequest = new();
    private CustomValidation? _customValidation;
    private bool BusySubmitting { get; set; }

    [Inject]
    private IUsersClient UsersClient { get; set; } = default!;

    private string Tenant { get; set; } = MultitenancyConstants.Root.Id;

    private async Task SubmitAsync()
    {
        BusySubmitting = true;

        await ApiHelper.ExecuteCallGuardedAsync(
            () => UsersClient.ForgotPasswordAsync(Tenant, _forgotPasswordRequest),
            Snackbar,
            _customValidation);

        BusySubmitting = false;
    }
}