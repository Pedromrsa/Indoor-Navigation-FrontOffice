using System.Text;
using System.Text.Json;

namespace IndoorMappingApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage(),true);
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecoverAccountPage(), true);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                DisplayAlert("Login Error", "Please enter both email and password.", "OK");
                return;
            }
            else
            {

                var client = new HttpClient();
                var url = "https://isepindoornavigationapi-vgq7.onrender.com/api/Auth/login";


                var requestData = new
                {
                    email = email,
                    password = password
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        await Shell.Current.GoToAsync("mainmenu", true, new Dictionary<string, object>
                        {
                            { "IsGuest", false }
                        });
                    }
                    else
                    {
                        await DisplayAlert("Login Error", $"Invalid Credentials", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Exceção: {ex.Message}", "OK");
                }
            }
        }
    }
}
