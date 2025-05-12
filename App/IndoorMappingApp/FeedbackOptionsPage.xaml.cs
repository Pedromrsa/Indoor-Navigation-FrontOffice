using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp
{

    public partial class FeedbackOptionsPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<string> PathOptions { get; set; }
        private ObservableCollection<PathDetail> _paths = new ObservableCollection<PathDetail>();
        private PathDetail _selectedPath;
        public PathDetail SelectedPath
        {
            get => _selectedPath;
            set
            {
                _selectedPath = value;
                OnPropertyChanged();
            }
        }

        public FeedbackOptionsPage()
        {
            InitializeComponent();
            PathOptions = new ObservableCollection<string>();
            BindingContext = this;
            IsepNumber.Text = ActiveUser.UserNumber;

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

        private async void OnSendFeedbackClicked(object sender, EventArgs e)
        {
            var lrm = LocalizationResourceManager.Instance;
            var api = new IndoorApiService();

            if (ActiveUser.UserId == null || ActiveUser.UserId == "0")
            {
                await DisplayAlert(
                    lrm["Registration_Failure_Title"],
                    lrm["UserNotAuthenticated"],
                    lrm["Button_OK"]);
                return;
            }


            var selectedFeedback = new List<string>();

            var selectedPathOption = PathPicker.SelectedItem as PathDetail;

            if (Option1.IsChecked) selectedFeedback.Add(lrm["Option1_Text"]);
            if (Option2.IsChecked) selectedFeedback.Add((lrm["Option2_Text"]));
            if (Option3.IsChecked) selectedFeedback.Add((lrm["Option3_Text"]));
            if (Option4.IsChecked) selectedFeedback.Add((lrm["Option4_Text"]));
            if (Option5.IsChecked) selectedFeedback.Add((lrm["Option5_Text"]));

            string result = string.Join("\n", selectedFeedback);

            if (result == "")
            {
                await DisplayAlert(
                    lrm["Registration_Failure_Title"],
                    lrm["ChooseOneOption"],
                    lrm["Button_OK"]);
                return;
            }
            var feedback = new
            {
                usuarioId = ActiveUser.UserId,
                caminhoId = selectedPathOption == null ? 999 : selectedPathOption.Id,
                avaliacao = 1,
                comentario = result,
            };

            try
            {
                var httpClient = new HttpClient();
                var url = "https://isepindoornavigationapi-vgq7.onrender.com/api/FeedbackCaminhos";

                var json = JsonSerializer.Serialize(feedback);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert(
                        lrm["Registration_Success_Title"],
                        lrm["FeedbackSubmitted"], 
                        lrm["Button_OK"]);
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    await DisplayAlert(
                        lrm["Registration_Failure_Title"],
                        lrm["Request_Failed"],
                        lrm["Button_OK"]);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }


        }

        private async void OnBurgerMenuClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("options");
        }


    }


}
