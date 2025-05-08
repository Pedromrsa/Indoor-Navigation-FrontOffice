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
        var lrm = LocalizationResourceManager.Instance;

        if (IsValidEmail(emailEntry.Text))
        {
            var apiService = new IndoorApiService();

            bool response =  await apiService.ValidateEmailExistance(emailEntry.Text);

            if (response)
            {
                //apiService = new IndoorApiService();
                //bool isEmailSent = await apiService.RequestRecoveryTokenAsync(emailEntry.Text);

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
                //await DisplayAlert("Request Failure", "The email provided does not have a registered account.", "OK");
                await DisplayAlert(
                   lrm["Request_Failed"],
                   lrm["RecoverAccount_Label_EnterEmail"],
                   lrm["Button_OK"]);
            }
        }
        else
        {
            //await DisplayAlert("Validation Error", "Please enter your email or recheck its format.", "OK");
            await DisplayAlert(
                   lrm["ValidationError_Title"],
                   lrm["ValidationError_Email"],
                   lrm["Button_OK"]);
            return;
        }
    }
    async void OnVerifyClicked(object sender, EventArgs e)
    {
        var lrm = LocalizationResourceManager.Instance;

        var apiService = new IndoorApiService();
        bool isValidToken = await apiService.ValidateRecoveryTokenAsync(tokenEntry.Text);

        if (isValidToken)
        {
            await Navigation.PushAsync(new RecoverPasswordPage(tokenEntry.Text), true);
        }
        else
        {
            //await DisplayAlert("Validation Error", "Wrong token provided. Please provide a valid token", "OK");
            await DisplayAlert(
                    lrm["ValidationError_Title"],
                    lrm["ValidationError_Token"],
                    lrm["Button_OK"]);
        }
    }

    async void OnBackToMenuClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(), true);
    }

    private bool IsValidEmail(string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            var pattern = @"^[^@\s]+@isep\.ipp\.pt$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        else
        {
            return false;
        }
    }

}