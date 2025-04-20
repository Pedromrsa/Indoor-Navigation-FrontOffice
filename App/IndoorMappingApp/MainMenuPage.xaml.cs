using IndoorMappingApp.Scripts;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp;

public partial class MainMenuPage : ContentPage, IQueryAttributable
{
    private Dictionary<long, PointF> _infraIdToPixel = new();

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
    }

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

    private async void OnB304Clicked(object sender, EventArgs e)
    {
        await MostrarCaminhoParaDestino(9);
    }

    private async void OnB404Clicked(object sender, EventArgs e)
    {
        await MostrarCaminhoParaDestino(10);
    }

    private async Task InicializarCoordenadasInfraestruturasAsync()
    {
        var api = new IndoorApiService();
        var infraestruturas = await api.GetInfraestruturasAsync();

        _infraIdToPixel = infraestruturas.ToDictionary(
            infra => infra.InfraestruturaId,
            infra => MapGeolocation.ConvertToPixel(infra.Latitude, infra.Longitude)
        );
    }

    private async Task MostrarCaminhoParaDestino(long destinoId)
    {
        var api = new IndoorApiService();
        var resultado = await api.ObterMelhorCaminhoAsync(destinoId);

        if (resultado == null || resultado.InfraestruturasIds == null || !resultado.InfraestruturasIds.Any())
        {
            await DisplayAlert("Erro", "Não foi possível obter o caminho.", "OK");
            return;
        }

        if (_infraIdToPixel.Count == 0)
            await InicializarCoordenadasInfraestruturasAsync();

        var pontos = resultado.InfraestruturasIds
            .Where(InfraestruturaToPixelInMap.Pixeis.ContainsKey)
            .Select(id =>
            {
                var p = InfraestruturaToPixelInMap.Pixeis[id];
                return new Point((int)p.X, (int)p.Y);
            })
            .ToList();

        var novaImagem = await CaminhoMapPainter.PintarCaminhoAsync("mapa_isep.png", pontos);
        ImagemMapa.Source = novaImagem;


        await DisplayAlert("Caminho", resultado.Mensagem, "OK");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_infraIdToPixel.Count == 0)
            await InicializarCoordenadasInfraestruturasAsync();
    }
}
