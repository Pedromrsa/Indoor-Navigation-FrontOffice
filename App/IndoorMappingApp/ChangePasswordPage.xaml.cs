using IndoorMappingApp.Scripts.DTOs;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp;

public partial class ChangePasswordPage : ContentPage
{
    private readonly IndoorApiService _api = new IndoorApiService();
    public ChangePasswordPage()
	{
		InitializeComponent();
	}

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        string? email = EmailEntry.Text?.Trim();
        string oldPassword = OldPasswordEntry.Text;
        string newPassword = NewPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        var dto = new ChangePasswordDTO
        {
            email = email,
            oldPassword = oldPassword,
            newPassword = newPassword
        };

        var result = await _api.UpdatePasswordAsync(dto);

        if (result != null && result.Success)
        {
            await DisplayAlert("Success", result.Message ?? "Password changed successfully.", "OK");
            await Navigation.PopAsync(); // or Navigate to LoginPage
        }
        else
        {
            await DisplayAlert("Error", result?.Message ?? "Failed to change password.", "OK");
        }
    }
}