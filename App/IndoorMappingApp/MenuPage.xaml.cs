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

    async void OnGuestClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new MainPage()); // or main app page
}