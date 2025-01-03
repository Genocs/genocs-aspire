﻿using MudBlazor;

namespace Genocs.BlazorAspire.Infrastructure.Themes;

public class LightTheme : MudTheme
{
    public LightTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = CustomColors.Light.Primary,
            Secondary = CustomColors.Light.Secondary,
            Background = CustomColors.Light.Background,
            AppbarBackground = CustomColors.Light.AppbarBackground,
            AppbarText = CustomColors.Light.AppbarText,
            DrawerBackground = CustomColors.Light.Background,
            DrawerText = "rgba(0,0,0, 0.7)",
            Success = CustomColors.Light.Primary,
            TableLines = "#e0e0e029",
            OverlayDark = "hsl(0deg 0% 0% / 75%)"
        };

        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "5px"
        };

        Typography = CustomTypography.GNXTypography;
        Shadows = new Shadow();
        ZIndex = new ZIndex() { Drawer = 1300 };
    }
}