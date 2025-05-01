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

        LanguagePicker.ItemsSource = new[] { "English", "Portuguese" };
        LimitationPicker.ItemsSource = new[] { "Tetraplegic", "Paraplegia" };
    }

    async void OnRegisterClicked(object sender, EventArgs e)
    {
        var dto = new RegisterRequestDTO
        {
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text,
            Limitation = LimitationPicker.SelectedIndex,  
            Language = LanguagePicker.SelectedIndex     
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
