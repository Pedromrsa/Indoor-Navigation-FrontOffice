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

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        var lrm = LocalizationResourceManager.Instance;

        string newPassword = NewPasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // If new password is provided, ensure both fields are filled and match
        if (!string.IsNullOrWhiteSpace(newPassword) && !string.IsNullOrWhiteSpace(confirmPassword))
        {
            if (newPassword != confirmPassword)
            {
                //await DisplayAlert("Validation Error", "Passwords do not match.", "OK");
                await DisplayAlert(
                    lrm["ValidationError_Title"],
                    lrm["RecoverPassword_MismatchMessage"],
                    lrm["Button_OK"]);
                return;
            }
        }
        else
        {
            // If new password is blank, do not include it in the update
            newPassword = null; // Or keep it as null to retain the previous value
        }

        // If either Limitation or Language is not selected, retain the previous values
        int? limitation = (LimitationPicker.SelectedIndex >= 0) ? LimitationPicker.SelectedIndex : (int?)null;
        int? language = (LanguagePicker.SelectedIndex >= 0) ? LanguagePicker.SelectedIndex : (int?)null;

        var dto = new UpdateAccountRequestDTO
        {
            NewPassword = newPassword,
            Limitation = (int)limitation,
            Language = (int)language
        };

        var result = await _api.UpdateAccountSettingsAsync(dto);

        if (result != null && result.Success)
        {
            //await DisplayAlert("Success", result.Message ?? "Settings updated successfully.", "OK");
            await DisplayAlert(
                    lrm["Registration_Success_Title"],
                    lrm["SettingsSucess"],
                    lrm["Button_OK"]);
            return;
        }
        else
        {
            //await DisplayAlert("Error", result?.Message ?? "Update failed", "OK");
            await DisplayAlert(
                    lrm["Error"],
                    lrm["SettingsFailed"],
                    lrm["Button_OK"]);
            return;
        }
    }
}