using IndoorMappingApp.Scripts.DTOs;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp;

public partial class AccountSettings : ContentPage
{
    readonly IndoorApiService _api = new IndoorApiService();

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        string newPassword = NewPasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Validation Error", "Please fill in both password fields.", "OK");
            return;
        }

        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Validation Error", "Passwords do not match.", "OK");
            return;
        }

        if (LimitationPicker.SelectedIndex < 0 || LanguagePicker.SelectedIndex < 0)
        {
            await DisplayAlert("Validation Error", "Please select both limitation and language.", "OK");
            return;
        }

        var dto = new UpdateAccountRequestDTO
        {
            NewPassword = newPassword,
            Limitation = LimitationPicker.SelectedIndex,
            Language = LanguagePicker.SelectedIndex
        };

        var result = await _api.UpdateAccountSettingsAsync(dto);

        if (result != null && result.Success)
        {
            await DisplayAlert("Success", result.Message ?? "Settings updated successfully.", "OK");
        }
        else
        {
            await DisplayAlert("Error", result?.Message ?? "Update failed", "OK");
        }
    }
}