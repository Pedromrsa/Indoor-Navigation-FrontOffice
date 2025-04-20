using IndoorMappingApp.Scripts;
using Microsoft.Maui.Controls;

namespace IndoorMappingApp;

public partial class MainMenuPage : ContentPage, IQueryAttributable
{
    private CaminhoDrawable _drawable = new CaminhoDrawable();

    private bool _isGuest;
    public bool IsGuest
    {
        get => _isGuest;
        set
        {
            _isGuest = value;
            OptionsButton.IsVisible = !_isGuest;
        }
    }

    public MainMenuPage()
    {
        InitializeComponent();

        CaminhoView.Drawable = _drawable;

        _drawable.Pontos = new List<PointF>
        {
            new PointF(50, 50),
            new PointF(100, 150),
            new PointF(200, 250)
        };

        CaminhoView.Invalidate();
    }

    // Aqui recebe-se os parâmetros do Shell
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("IsGuest"))
        {
            IsGuest = Convert.ToBoolean(query["IsGuest"]);
        }
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
