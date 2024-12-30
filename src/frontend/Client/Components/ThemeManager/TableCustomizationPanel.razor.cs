using Genocs.BlazorAspire.Infrastructure.Notifications;
using Genocs.BlazorAspire.Infrastructure.Preferences;
using Microsoft.AspNetCore.Components;

namespace Genocs.BlazorAspire.Client.Components.ThemeManager;

public partial class TableCustomizationPanel
{
    [Parameter]
    public bool IsDense { get; set; }

    [Parameter]
    public bool IsStriped { get; set; }

    [Parameter]
    public bool HasBorder { get; set; }

    [Parameter]
    public bool IsHoverable { get; set; }

    [Inject]
    protected INotificationPublisher Notifications { get; set; } = default!;

    private GnxTablePreference _tablePreference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference clientPreference)
        {
            _tablePreference = clientPreference.TablePreference;
        }

        IsDense = _tablePreference.IsDense;
        IsStriped = _tablePreference.IsStriped;
        HasBorder = _tablePreference.HasBorder;
        IsHoverable = _tablePreference.IsHoverable;
    }

    [Parameter]
    public EventCallback<bool> OnDenseSwitchToggled { get; set; }

    [Parameter]
    public EventCallback<bool> OnStripedSwitchToggled { get; set; }

    [Parameter]
    public EventCallback<bool> OnBorderdedSwitchToggled { get; set; }

    [Parameter]
    public EventCallback<bool> OnHoverableSwitchToggled { get; set; }

    private async Task ToggleDenseSwitchAsync()
    {
        _tablePreference.IsDense = !_tablePreference.IsDense;
        await OnDenseSwitchToggled.InvokeAsync(_tablePreference.IsDense);
        await Notifications.PublishAsync(_tablePreference);
    }

    private async Task ToggleStripedSwitchAsync()
    {
        _tablePreference.IsStriped = !_tablePreference.IsStriped;
        await OnStripedSwitchToggled.InvokeAsync(_tablePreference.IsStriped);
        await Notifications.PublishAsync(_tablePreference);
    }

    private async Task ToggleBorderedSwitchAsync()
    {
        _tablePreference.HasBorder = !_tablePreference.HasBorder;
        await OnBorderdedSwitchToggled.InvokeAsync(_tablePreference.HasBorder);
        await Notifications.PublishAsync(_tablePreference);
    }

    private async Task ToggleHoverableSwitchAsync()
    {
        _tablePreference.IsHoverable = !_tablePreference.IsHoverable;
        await OnHoverableSwitchToggled.InvokeAsync(_tablePreference.IsHoverable);
        await Notifications.PublishAsync(_tablePreference);
    }
}