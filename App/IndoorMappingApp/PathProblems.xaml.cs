using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text;

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
    
    private string elevatorDecription;

    public PathProblems()
    {
        InitializeComponent();
        LoadPathsAsync();
        LoadElevatorsAsync();
        IsepNumber.Text = ActiveUser.UserNumber;
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

                // Filtra caminhos acessíveis e elimina duplicatas de DisplayName
                var accessiblePaths = pathList
                    .Where(p => p.Acessivel)
                    .GroupBy(p => p.DisplayName) // Agrupa por DisplayName
                    .Select(g => g.First()) // Seleciona o primeiro de cada grupo
                    .ToList();

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

    private async void OnBurgerMenuClicked(object sender, EventArgs e)
    {
        SelectedPath = null;
        ElevatorProblemCheckBox.IsChecked = false;
        ObstructionCheckBox.IsChecked = false;
        OtherProblemCheckBox.IsChecked = false;
        await Shell.Current.GoToAsync("options");
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
                ElevatorPicker.ItemDisplayBinding = new Binding("DisplayName");
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

    private void OnProblemOptionCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value) return; // Ignora quando desmarcado

        var changedCheckBox = (CheckBox)sender;

        // Desmarca todas as outras
        if (changedCheckBox != ElevatorProblemCheckBox)
        {
            ElevatorProblemCheckBox.IsChecked = false;
            elevatorDecription = null;
        }
        else
        {
            if (ElevatorPicker.SelectedItem is Infrastructure selectedElevator)
            {
                Console.WriteLine($"Elevador selecionado: {selectedElevator.Descricao}");

                elevatorDecription = selectedElevator.Descricao;
            }
        }

        if (changedCheckBox != ObstructionCheckBox)
            ObstructionCheckBox.IsChecked = false;

        if (changedCheckBox != OtherProblemCheckBox)
            OtherProblemCheckBox.IsChecked = false;

        // Exemplo opcional: ativar/desativar o Entry de "Other problem"
        OtherProblemEntry.IsEnabled = changedCheckBox == OtherProblemCheckBox;
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
        SelectedPath = null;
        ElevatorProblemCheckBox.IsChecked = false;
        ObstructionCheckBox.IsChecked = false;
        OtherProblemCheckBox.IsChecked = false;
        await Shell.Current.GoToAsync("options");
    }

    private async void OnSubmitButton(object sender, EventArgs e)
    {
        // Verifique se o usuário e caminho estão definidos
        if (ActiveUser.UserId == null || ActiveUser.UserId == "0")
        {
            await DisplayAlert("Erro", "Usuário não autenticado.", "OK");
            return;
        }

        if (_selectedPath == null)
        {
            await DisplayAlert("Erro", "Selecione um caminho.", "OK");
            return;
        }

        string comentario = string.Empty;

        if (ElevatorProblemCheckBox.IsChecked)
        {
            comentario = elevatorDecription + " is Broken";
        }
        else if (ObstructionCheckBox.IsChecked)
        {
            comentario = "Path obstruct"; 
        }
        else if (OtherProblemCheckBox.IsChecked)
        {
            comentario = OtherProblemEntry.Text; 
        }



        var feedbackCaminho = new
        {
            usuarioId = ActiveUser.UserId,
            caminhoId = _selectedPath.Id,
            avaliacao = 1,
            comentario = comentario,
        };

        try
        {
            var httpClient = new HttpClient();
            var url = "https://isepindoornavigationapi-vgq7.onrender.com/api/FeedbackCaminhos";

            var json = JsonSerializer.Serialize(feedbackCaminho);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Sucesso", "Feedback enviado com sucesso!", "OK");
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Erro", $"Erro ao enviar feedback: {errorMsg}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exceção", ex.Message, "OK");
        }
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

    public string DisplayName => $"{Descricao} - Elevator is broken";

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
