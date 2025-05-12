using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp;

public partial class RecoverPasswordPage : ContentPage
{
    public string Token { get; set; }
    public RecoverPasswordPage(string token)
    {
        InitializeComponent();
        this.Token = token;
    }

    async void OnChangePasswordButtonClicked(object sender, EventArgs e)
    {
        var lrm = LocalizationResourceManager.Instance;

        var apiService = new IndoorApiService();
        bool isValidToken = await apiService.ResetPasswordAsync(Token, passwordEntry.Text);

        if (isValidToken)
        {
            //await DisplayAlert("Succcess", "Password was successfuly changed!", "OK");
            await DisplayAlert(
                   lrm["Success"],
                   lrm["PasswordChangedSucess"],
                   lrm["Button_OK"]);
            await Navigation.PushAsync(new LoginPage(), true);
        }
        else
        {
            //await DisplayAlert("Error", "Error changing password!", "OK");
            await DisplayAlert(
                   lrm["Error"],
                   lrm["PasswordChangeFailed"],
                   lrm["Button_OK"]);
            return;
        }
    }

    private void ConfirmPasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var lrm = LocalizationResourceManager.Instance;
        if (!string.IsNullOrEmpty(confirmPasswordEntry.Text))
        {
            if (passwordEntry.Text == confirmPasswordEntry.Text)
            {
                //matchLabel.Text = "Passwords match!";
                matchLabel.Text = lrm["RecoverPassword_MatchMessage"];
                matchLabel.TextColor = Colors.Green;
                changePasswordButton.IsEnabled = true;
            }
            else
            {
                //matchLabel.Text = "Passwords don't match!";
                matchLabel.Text = lrm["RecoverPassword_MismatchMessage"];
                matchLabel.TextColor = Colors.Red;
                changePasswordButton.IsEnabled = false;
            }

            matchLabel.IsVisible = true;
        }
        else
        {
            matchLabel.IsVisible = false;
        }
    }
}