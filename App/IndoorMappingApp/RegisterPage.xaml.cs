namespace IndoorMappingApp;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        // Example languages
        LanguagePicker.ItemsSource = new[]
        {
            "English", "Spanish", "French", "German"
        };
    }

    async void OnRegisterClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;
        var limitation = LimitationEditor.Text;
        var language = LanguagePicker.SelectedItem as string;

        // TODO: Add validation and backend call here
        await DisplayAlert(
            "Registered",
            $"Email: {email}\nLanguage: {language}\nLimitation: {limitation}",
            "OK");

        // Optionally pop back to menu
        await Navigation.PopAsync();
    }
}
