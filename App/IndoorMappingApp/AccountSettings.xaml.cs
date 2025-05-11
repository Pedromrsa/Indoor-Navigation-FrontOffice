using System.Globalization;
using IndoorMappingApp.Scripts.DTOs;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp;

public partial class AccountSettings : ContentPage
{
    readonly IndoorApiService _api = new IndoorApiService();
    public AccountSettings()
    {
        InitializeComponent();
    }

    private async void OnPasswordClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePasswordPage());
    }

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        // get selected limitation
        int? limitation = (LimitationPicker.SelectedIndex >= 0) ? LimitationPicker.SelectedIndex + 1 : (int?)null;

        // get selected language
        var selectedCulture = GetSelectedLanguage();

        if (limitation == null && selectedCulture == null)
        {
            await DisplayAlert("No Changes", "Please select a value to update.", "OK");
            return;
        }

        // Retrieve user ID from ActiveUser
        if (!int.TryParse(ActiveUser.UserId, out int userId))
        {
            await DisplayAlert("Error", "User ID not found or invalid. Please log in again.", "OK");
            return;
        }

        if (limitation == null && selectedCulture != null)
        {
            LocalizationResourceManager.Instance.SetCulture(selectedCulture);
            Preferences.Set("AppLanguage", selectedCulture.Name);
            await DisplayAlert("Success", "Language updated successfully.", "OK");
            return;
        }

        var dto = new UpdateAccountRequestDTO
        {
            usuarioId = int.Parse(ActiveUser.UserId),
            nome = ActiveUser.UserName,
            email = ActiveUser.UserEmail,
            tipoUsuarioId = GetTipoUsuarioId(ActiveUser.UserType),
            mobilidadeId = (int)limitation
        };

        var result = await _api.UpdateUserInfoAsync(userId, dto);

        if (result != null && result.Success)
        {
            await DisplayAlert("Success", result.Message ?? "Settings updated successfully.", "OK");
        }
        else
        {
            await DisplayAlert("Error", result?.Message ?? "Update failed.", "OK");
        }
    }

    private int GetTipoUsuarioId(string userType)
    {
        return userType.ToLower() switch
        {
            "Admin" => 1,
            "User" => 2,
            "Editor" => 3,
            "Reader" => 4,
            _ => 4
        };
    }

    private CultureInfo? GetSelectedLanguage()
    {
        return LanguagePicker.SelectedIndex switch
        {
            0 => new CultureInfo("en-US"),
            1 => new CultureInfo("pt-PT"),
            _ => null
        };
    }

}