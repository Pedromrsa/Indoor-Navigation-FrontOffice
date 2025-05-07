using IndoorMappingApp.Scripts.Services;
using IndoorMappingApp.Scripts.DTOs;
namespace IndoorMappingApp;

public partial class RegisterPage : ContentPage
{
    readonly IndoorApiService _api;
    public RegisterPage()
    {
        InitializeComponent();
        _api = new IndoorApiService();

        LimitationPicker.ItemsSource = new[] { "Normal", "Paraplegic", "Tetraplegic" };
        NameEntry.TextChanged += (s, e) => ValidateForm();
        EmailEntry.TextChanged += (s, e) => ValidateForm();
        PasswordEntry.TextChanged += (s, e) => ValidateForm();
        LimitationPicker.SelectedIndexChanged += (s, e) => ValidateForm();
    }

    void ValidateForm()
    {
        bool isValid =
            !string.IsNullOrWhiteSpace(NameEntry.Text) &&
            !string.IsNullOrWhiteSpace(EmailEntry.Text) &&
            !string.IsNullOrWhiteSpace(PasswordEntry.Text) &&
            LimitationPicker.SelectedIndex >= 0;

        RegisterButton.IsEnabled = isValid;
    }

    async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Validation Error", "Please enter your name.", "OK");
            return;
        }

        var dto = new RegisterRequestDTO
        {
            nome = NameEntry.Text,
            email = EmailEntry.Text,
            password = PasswordEntry.Text,
            tipoId = 4,
            mobilidadeId = LimitationPicker.SelectedIndex + 1
        };

        var result = await _api.RegisterAsync(dto); 
        if (result != null && result.IsSuccessStatusCode)
        {

            await DisplayAlert("Registered", "Ok", "Ok");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Registration failed", "OK");
        }
    }

    async void OnLoginClicked(object sender, EventArgs e)
    {
         await Navigation.PushAsync(new LoginPage());
    }

}
