namespace IndoorMappingApp;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        
        LanguagePicker.ItemsSource = new[]
        {
            "English", "Portuguese"
        };
        LimitationPicker.ItemsSource = new[] { "Tetraplegic", "Paraplegia" };
    }

    async void OnRegisterClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;
        var limitation = LimitationPicker.SelectedItem as string;
        var language = LanguagePicker.SelectedItem as string;

        // TODO: Add validation and backend call here
        await DisplayAlert(
            "Registered",
            $"Email: {email}\nLanguage: {language}\nLimitation: {limitation}",
            "OK");

        // Optionally navigate back to the previous page
        await Navigation.PopAsync();
    }

}
