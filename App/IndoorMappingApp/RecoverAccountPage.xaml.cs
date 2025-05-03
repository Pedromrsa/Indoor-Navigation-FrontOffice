using IndoorMappingApp.Scripts.Services;
using System.Text.RegularExpressions;

namespace IndoorMappingApp;

public partial class RecoverAccountPage : ContentPage
{
    public RecoverAccountPage()
    {
        InitializeComponent();
    }

    async void OnSendEmailClicked(object sender, EventArgs e)
    {
        if (IsValidEmail(emailEntry.Text))
        {
            var apiService = new IndoorApiService();
            bool isEmailSent = await apiService.RequestRecoveryTokenAsync(emailEntry.Text);
            if (isEmailSent)
            {
                sendEmailButton.IsVisible = false;
                verifyButton.IsVisible = true;
                sendEmailLabel.IsVisible = false;
                emailLabel.IsVisible = false;
                tokenLabel.IsVisible = true;
                emailFrame.IsVisible = false;
                tokenFrame.IsVisible = true;
            }
            else
            {
                emailEntry.Text = string.Empty;
                await DisplayAlert("Request Failure", "The email provided does not have a registered account.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Validation Error", "Please enter your email or recheck its format.", "OK");
            return;
        }
    }

    async void OnVerifyClicked(object sender, EventArgs e)
    {
        var apiService = new IndoorApiService();
        bool isValidToken = await apiService.ValidateRecoveryTokenAsync(tokenEntry.Text);

        if (isValidToken)
        {
            //await Navigation.PushAsync(new ResetPasswordPage(tokenEntry.Text), true);
        }
        else
        {
            await DisplayAlert("Validation Error", "Wrong token provided. Please provide a valid token", "OK");
        }
    }

    private bool IsValidEmail(string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        else
        {
            return false;
        }
    }
}