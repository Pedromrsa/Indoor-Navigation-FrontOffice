using IndoorMappingApp.Scripts;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp;

public partial class MainMenuPage : ContentPage, IQueryAttributable
{
    private Dictionary<long, PointF> _infraIdToPixel = new();
    private readonly string _mapFileName = "mapa_isep.png";


    private bool _isGuest;
    public bool IsGuest
    {
        get => _isGuest;
        set
        {
            _isGuest = value;
            OptionsButton.IsVisible = !_isGuest;
            //!_isGuest;
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
    
    private async Task MostrarCaminhoParaDestino(long destinoId)
    {
        var lrm = LocalizationResourceManager.Instance;

        var api = new IndoorApiService();
        var resultado = await api.ObterMelhorCaminhoAsync(destinoId);

        if (resultado == null || resultado.InfraestruturasIds == null || !resultado.InfraestruturasIds.Any())
        {
            //await DisplayAlert("Erro", "Não foi possível obter o caminho.", "OK");
            await DisplayAlert(
                    lrm["Registration_Failure_Title"],
                    lrm["Path_Message"],
                    lrm["Button_OK"]);
            return;
        }

        // Busca todas as infraestruturas com tipo e descrição
        var infraestruturas = await api.GetInfraestruturasAsync();
        var steps = new List<string>();

        for (int i = 0; i < resultado.InfraestruturasIds.Count - 1; i++)
        {
            var origemId = resultado.InfraestruturasIds[i];
            var destinoStepId = resultado.InfraestruturasIds[i + 1];

            var origem = infraestruturas.FirstOrDefault(x => x.Id == origemId);
            var destino = infraestruturas.FirstOrDefault(x => x.Id == destinoStepId);

            if (origem != null && destino != null)
            {
                steps.Add($"De {origem.TipoInfraestrutura} {origem.Descricao} ({origem.Piso}) A {destino.TipoInfraestrutura} {destino.Descricao} ({origem.Piso})");
            }
        }

        // Atualiza o texto no ecrã
        PercursoLabel.Text = string.Join("\n", steps);

        // Continua pintando o mapa
        var pontos = resultado.InfraestruturasIds
            .Where(InfraestruturaToPixelInMap.Pixeis.ContainsKey)
            .Select(id => InfraestruturaToPixelInMap.Pixeis[id])
            .ToList();

        try
        {
            var novaImagem = await CaminhoMapPainter.PintarCaminhoAsync(ImageSource.FromFile(_mapFileName), pontos);
            ImagemMapa.Source = novaImagem;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao pintar caminho", ex.Message, "OK");
        }
    }




}
