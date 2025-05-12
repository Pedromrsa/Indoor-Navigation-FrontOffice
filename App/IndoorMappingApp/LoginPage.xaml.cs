using System.Text;
using System.Text.Json;
using IndoorMappingApp;

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

            await PerformLogin(email, password);
        }


        private async Task PerformLogin(string email, string password)
        {
            var lrm = LocalizationResourceManager.Instance;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert(lrm["Login_Failure_Title"], lrm["Login_Validation_Error"], lrm["Button_OK"]);
                return;
            }

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
                    ActiveUser.UserEmail = email;
                    await LocalStorageService.SaveDataAsync("email.txt", email);
                    await LocalStorageService.SaveDataAsync("password.txt", password);  
                    var email1 = Preferences.Default.Get<String>("email", null);


                    var userUrl = "https://isepindoornavigationapi-vgq7.onrender.com/api/Usuarios";
                    var userResponse = await client.GetAsync(userUrl);

                    if (userResponse.IsSuccessStatusCode)
                    {
                        var userJson = await userResponse.Content.ReadAsStringAsync();
                        var usuarios = JsonSerializer.Deserialize<List<User>>(userJson);
                        var usuario = usuarios.FirstOrDefault(u => u.email.Equals(email, StringComparison.OrdinalIgnoreCase));

                        ActiveUser.UserId = usuario.usuarioId;
                        ActiveUser.UserName = usuario.nome;
                        ActiveUser.UserMobilityType = usuario.mobilidadeTipo;
                        ActiveUser.UserType = usuario.tipoUsuario;
                        await Shell.Current.GoToAsync("mainmenu", true, new Dictionary<string, object>
                {
                    { "IsGuest", false }
                });
                    }
                }
                else
                {
                    await DisplayAlert(lrm["Login_Failure_Title"], lrm["Login_Invalid_Credentials"], lrm["Button_OK"]);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Exceção: {ex.Message}", "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var email = await LocalStorageService.GetDataAsync("email.txt");
            var password = await LocalStorageService.GetDataAsync("password.txt");

            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Utilizador encontrado","Por favor aguarde... ", "OK");
                await PerformLogin(email, password);  // Tentar o login automaticamente
            }
        }

    }
}
public static class ActiveUser
{
    public static string UserId { get; set; }
    public static string UserName { get; set; }
    public static string UserEmail { get; set; }
    public static string UserMobilityType { get; set; }
    public static string UserType { get; set; }
    public static string UserNumber => UserEmail?.Split('@')[0];
}
public class User
{
    public string usuarioId { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public string mobilidadeTipo { get; set; }
    public string tipoUsuario { get; set; }
}