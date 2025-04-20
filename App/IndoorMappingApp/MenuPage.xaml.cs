namespace IndoorMappingApp;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
    }

    async void OnLoginClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new LoginPage());

    async void OnRegisterClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new RegisterPage());

    private async void OnGuestClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("mainmenu", true,
            new Dictionary<string, object>
            {
                { "IsGuest", true }
            });
    }
}