using System.Collections.ObjectModel;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp
{
    public partial class AvailableRoutesPage : ContentPage
    {
        public ObservableCollection<RouteInfo> Routes { get; set; }

        public AvailableRoutesPage()
        {
            InitializeComponent();
            Routes = new ObservableCollection<RouteInfo>();
            BindingContext = this;

            LoadRoutes();
        }

        private async void LoadRoutes()
        {

            // Fetch routes for B311
            // var b311Routes = await apiService.GetRoutesForRoomAsync("B311");
            var steps = await GetStringPathToRoom(9);
            Routes.Add(new RouteInfo
            {
                RoomName = "B311",
                RouteDetails = string.Join("\n", steps)
            });

            // Fetch routes for B404
            steps =  await GetStringPathToRoom(10);
            Routes.Add(new RouteInfo
            {
                RoomName = "B404",
                RouteDetails = string.Join("\n", steps)
            });
        }
        public async Task<List<String>> GetStringPathToRoom(int roomID)
        {
            var lrm = LocalizationResourceManager.Instance;

            var api = new IndoorApiService();
            var resultado = await api.ObterMelhorCaminhoAsync(roomID);
            var steps = new List<string>();


            if (resultado == null || resultado.InfraestruturasIds == null || !resultado.InfraestruturasIds.Any())
            {
                //await DisplayAlert("Erro", "Não foi possível obter o caminho.", "OK");
                await DisplayAlert(
                        lrm["Registration_Failure_Title"],
                        lrm["Path_Message"],
                        lrm["Button_OK"]);
                return steps;
            }

            // Busca todas as infraestruturas com tipo e descrição
            var infraestruturas = await api.GetInfraestruturasAsync();

            for (int i = 0; i < resultado.InfraestruturasIds.Count - 1; i++)
            {
                var origemId = resultado.InfraestruturasIds[i];
                var destinoStepId = resultado.InfraestruturasIds[i + 1];

                var origem = infraestruturas.FirstOrDefault(x => x.Id == origemId);
                var destino = infraestruturas.FirstOrDefault(x => x.Id == destinoStepId);

                if (origem != null && destino != null)
                {
                    steps.Add($"De {origem.TipoInfraestrutura} {origem.Descricao} ({origem.Piso}) A {destino.TipoInfraestrutura} {destino.Descricao} ({destino.Piso})");
                }
            }
            return steps;
        }
    }

    

    public class RouteInfo
    {
        public string RoomName { get; set; }
        public string RouteDetails { get; set; }
    }
}
