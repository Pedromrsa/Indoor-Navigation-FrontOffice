using IndoorMappingApp.Scripts;

namespace IndoorMappingApp;

[QueryProperty(nameof(IsGuest), "IsGuest")]
public partial class MainMenuPage : ContentPage
{
    private bool _isGuest;
    public bool IsGuest
    {
        get => _isGuest;
        set
        {
            _isGuest = value;

            // Evita NullReference se OptionsButton ainda não estiver pronto
            if (OptionsButton != null)
                OptionsButton.IsVisible = !_isGuest;
        }
    }

    private CaminhoDrawable _drawable = new CaminhoDrawable();

    public MainMenuPage()
    {
        InitializeComponent();
        CaminhoView.Drawable = _drawable;


        // Mostrar botão Options só se NÃO for Guest
        OptionsButton.IsVisible = !_isGuest;

        // Exemplo temporário: desenhar um caminho manual
        _drawable.Pontos = new List<PointF>
            {
        new PointF(50, 50),
        new PointF(100, 150),
        new PointF(200, 250)
    };

        CaminhoView.Invalidate(); // redesenha
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