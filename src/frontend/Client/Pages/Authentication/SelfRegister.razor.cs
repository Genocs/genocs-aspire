﻿using Genocs.BlazorAspire.Client.Components.Common;
using Genocs.BlazorAspire.Infrastructure.ApiClient;
using Genocs.BlazorAspire.Client.Shared;
using Genocs.BlazorAspire.Shared.MultiTenancy;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Genocs.BlazorAspire.Client.Pages.Authentication;

public partial class SelfRegister
{
    private readonly CreateUserRequest _createUserRequest = new();
    private CustomValidation? _customValidation;
    private bool BusySubmitting { get; set; }

    [Inject]
    private IUsersClient UsersClient { get; set; } = default!;

    private string Tenant { get; set; } = MultitenancyConstants.Root.Id;

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private async Task SubmitAsync()
    {
        BusySubmitting = true;

        string? successMessage = await ApiHelper.ExecuteCallGuardedAsync(
            () => UsersClient.SelfRegisterAsync(Tenant, _createUserRequest),
            Snackbar,
            _customValidation);

        if (successMessage != null)
        {
            Snackbar.Add(successMessage, Severity.Info);
            Navigation.NavigateTo("/login");
        }

        BusySubmitting = false;
    }

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }
}