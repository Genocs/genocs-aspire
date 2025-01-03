﻿namespace Genocs.BlazorAspire.Infrastructure.Preferences;

/// <summary>
/// The interface is used to define the preference of the user.
/// </summary>
public interface IPreference
{
    /// <summary>
    /// The language code of the user.
    /// </summary>
    public string LanguageCode { get; set; }
}