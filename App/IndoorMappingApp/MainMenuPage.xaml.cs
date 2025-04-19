namespace IndoorMappingApp;

public partial class MainMenuPage : ContentPage
{
    private readonly bool _isGuest;

    public MainMenuPage(bool isGuest = true)
    {
        InitializeComponent();
        _isGuest = isGuest;

        // Mostrar bot�o Options s� se N�O for Guest
        OptionsButton.IsVisible = !_isGuest;
    }

    private async void OnRoutesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("routes");
    }

    private async void OnOptionsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("options");
    }
}