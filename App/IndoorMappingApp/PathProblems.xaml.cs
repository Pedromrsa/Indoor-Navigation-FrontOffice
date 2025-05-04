using System.Collections.ObjectModel;
using System.Text.Json;

namespace IndoorMappingApp;

public partial class PathProblems : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private ObservableCollection<PathDetail> _paths = new ObservableCollection<PathDetail>();

    private PathDetail _selectedPath;
    public PathDetail SelectedPath
    {
        get => _selectedPath;
        set
        {
            _selectedPath = value;
            OnPropertyChanged();  // Notifica a UI para atualizar a seleção
        }
    }

    public PathProblems()
    {
        InitializeComponent();
        LoadPathsAsync();
        LoadElevatorsAsync(); // Corrigido nome e chamada
    }

    private async Task LoadPathsAsync()
    {
        try
        {
            string url = "https://isepindoornavigationapi-vgq7.onrender.com/api/Caminhos/detailed";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var pathList = JsonSerializer.Deserialize<List<PathDetail>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var accessiblePaths = pathList.Where(p => p.Acessivel).ToList();
                _paths = new ObservableCollection<PathDetail>(accessiblePaths);

                this.BindingContext = this;
                PathPicker.ItemsSource = _paths;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                await DisplayAlert("HTTP Error", $"Status: {response.StatusCode}\n{errorContent}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception", ex.Message, "OK");
        }
    }

    private async void LoadElevatorsAsync()
    {
        try
        {
            string url = "https://isepindoornavigationapi-vgq7.onrender.com/api/Infraestrutura/GetAll";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var infraList = JsonSerializer.Deserialize<List<Infrastructure>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var uniqueElevators = infraList
                    .Where(i => i.Acessivel && i.TipoInfraestrutura == "Elevador")
                    .GroupBy(i => i.Descricao) // Um por edifício
                    .Select(g => g.First())     // Seleciona um por grupo
                    .ToList();

                ElevatorPicker.ItemsSource = uniqueElevators;
                ElevatorPicker.ItemDisplayBinding = new Binding("Descricao"); // Mostra a descrição no Picker
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Erro ao buscar elevadores", errorContent, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void PathPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PathPicker.SelectedItem is PathDetail selectedPath)
        {
            Console.WriteLine($"Caminho selecionado: {selectedPath.DisplayName}");
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

// Modelos
public class Infrastructure
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Piso { get; set; }
    public bool Acessivel { get; set; }
    public string TipoInfraestrutura { get; set; }

    public override string ToString()
    {
        return $"{Descricao} (Piso {Piso}) - {TipoInfraestrutura}";
    }
}

public class PathDetail
{
    public int Id { get; set; }
    public int OrigemId { get; set; }
    public string OrigemDescricao { get; set; }
    public int OrigemPiso { get; set; }
    public string OrigemTipoInfraestrutura { get; set; }
    public int DestinoId { get; set; }
    public string DestinoDescricao { get; set; }
    public int DestinoPiso { get; set; }
    public string DestinoTipoInfraestrutura { get; set; }
    public int Distancia { get; set; }
    public bool Acessivel { get; set; }
    public string TipoAcessibilidade { get; set; }

    public string DisplayName => $"{OrigemDescricao} - {DestinoDescricao} : {TipoAcessibilidade}";
    public override string ToString()
    {
        return DisplayName;
    }
}
