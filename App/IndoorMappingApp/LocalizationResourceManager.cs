using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;                   // ⬅️ make ResourceManager type visible
using IndoorMappingApp.Resources.Strings;

public class LocalizationResourceManager : INotifyPropertyChanged
{
    private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;
    public static LocalizationResourceManager Instance { get; } = new();

    public event PropertyChangedEventHandler PropertyChanged;

    // rename so we don’t shadow the type name
    private static ResourceManager _resMgr
        => AppResources.ResourceManager;

    public string this[string key] =>
        _resMgr.GetString(key, _currentCulture)   // now compiles and returns localized text
            ?? $"!{key}!";

    public void SetCulture(CultureInfo culture)
    {
        if (culture == null || culture.Equals(_currentCulture))
            return;

        _currentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        AppResources.Culture = culture;
        OnPropertyChanged(string.Empty);
    }

    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
