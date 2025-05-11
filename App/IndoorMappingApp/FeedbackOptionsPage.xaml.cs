using System.Collections.ObjectModel;
using System.Text.Json;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp
{

    public partial class FeedbackOptionsPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<string> PathOptions { get; set; }
        private ObservableCollection<PathDetail> _paths = new ObservableCollection<PathDetail>();
        public FeedbackOptionsPage()
        {
            InitializeComponent();
            PathOptions = new ObservableCollection<string>();
            BindingContext = this;

            LoadPathsAsync();
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

        private async void LoadPathOptions()
        {
            var api = new IndoorApiService();
            var infraestruturas = await api.GetInfraestruturasAsync();
            var paths = new List<string> { "No" };

            if (infraestruturas != null && infraestruturas.Any())
            {
                for (int i = 0; i < infraestruturas.Count - 1; i++)
                {
                    var origem = infraestruturas[i];
                    var destino = infraestruturas[i + 1];

                    if (origem != null && destino != null)
                    {
                        paths.Add($"De {origem.TipoInfraestrutura} {origem.Descricao} ({origem.Piso}) A {destino.TipoInfraestrutura} {destino.Descricao} ({destino.Piso})");
                    }
                }
            }

            foreach (var path in paths)
            {
                PathOptions.Add(path);
            }
        }

        private async void OnSendFeedbackClicked(object sender, EventArgs e)
        {
            var lrm = LocalizationResourceManager.Instance;
            var api = new IndoorApiService();



            var selectedFeedback = new List<string>();

            var selectedPathOption = PathPicker.SelectedItem as string;

            if (Option1.IsChecked) selectedFeedback.Add(lrm["Option1_Text"]);
            if (Option2.IsChecked) selectedFeedback.Add((lrm["Option2_Text"]));
            if (Option3.IsChecked) selectedFeedback.Add((lrm["Option3_Text"]));
            if (Option4.IsChecked) selectedFeedback.Add((lrm["Option4_Text"]));
            if (Option5.IsChecked) selectedFeedback.Add((lrm["Option5_Text"]));

            string result = string.Join("\n", selectedFeedback);


            var done = await api.PostFeedback(0, selectedPathOption ?? "No", result);

            if (done) {
                //DisplayAlert("Feedback Submitted", result, "OK");
                await DisplayAlert(
                            lrm["Feedback"],
                            lrm["FeedbackSubmitted"],
                            lrm["Button_OK"]);
            }

            
        }

        private async void OnBurgerMenuClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("options");
        }


    }


}
