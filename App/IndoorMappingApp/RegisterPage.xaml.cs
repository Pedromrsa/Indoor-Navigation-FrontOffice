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

        LimitationPicker.ItemsSource = new[] { "Tetraplegic", "Paraplegic", "Normal" };
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
            name = NameEntry.Text,
            email = EmailEntry.Text,
            password = PasswordEntry.Text,
            tipoId = 4,  
            mobilidadeId = LimitationPicker.SelectedIndex     
        };

        var result = await _api.RegisterAsync(dto);
        if (result != null && result.Success)
        {
            await DisplayAlert("Registered", result.Message ?? "OK", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", result?.Message ?? "Registration failed", "OK");
        }
    }

    async void OnLoginClicked(object sender, EventArgs e)
    {
         await Navigation.PushAsync(new LoginPage());
    }

}
