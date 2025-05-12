using System.Collections.ObjectModel;
using System.Text.Json;
using IndoorMappingApp.Scripts.Services;
using IndoorMappingApp.Scripts.DTOs;

namespace IndoorMappingApp
{
    public partial class AvailableRoutesPage : ContentPage
    {
        public ObservableCollection<RouteInfo> Routes { get; set; }
        private readonly HttpClient _httpClient = new HttpClient();

        public AvailableRoutesPage()
        {
            InitializeComponent();
            Routes = new ObservableCollection<RouteInfo>();
            BindingContext = this;

            LoadAllRoutes();
        }

        private async void LoadAllRoutes()
        {
            // For B311 (destination 9)
            var b311Routes = await GetAllRoutesToRoom(9, "B311");
            foreach (var route in b311Routes)
                Routes.Add(route);

            // For B404 (destination 10)
            var b404Routes = await GetAllRoutesToRoom(10, "B404");
            foreach (var route in b404Routes)
                Routes.Add(route);
        }

        private async Task<List<RouteInfo>> GetAllRoutesToRoom(int destinationId, string roomName)
        {
            var allRoutes = new List<RouteInfo>();
            int[] origins = { 13, 1 };

            foreach (var origin in origins)
            {
                var url = $"https://isepindoornavigationapi-vgq7.onrender.com/api/Caminhos/todos-detalhados?origemId={origin}&destinoId={destinationId}";
                try
                {
                    var response = await _httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var routeResponse = JsonSerializer.Deserialize<AllRoutesResponseDTO>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (routeResponse?.caminhos != null)
                        {
                            int routeNumber = 1;
                            foreach (var route in routeResponse.caminhos)
                            {
                                var steps = route.Select(step =>
                                    $"De {step.OrigemTipoInfraestrutura} {step.OrigemDescricao} ({step.OrigemPiso}) " +
                                    $"A {step.DestinoTipoInfraestrutura} {step.DestinoDescricao} ({step.DestinoPiso})"
                                );
                                var origem = route.FirstOrDefault(x => x.OrigemId == origin);
                                allRoutes.Add(new RouteInfo
                                {
                                    RoomName = $"{roomName} (Origem {origem.OrigemTipoInfraestrutura} {origem.OrigemDescricao}, Rota {routeNumber})",
                                    RouteDetails = string.Join("\n", steps)
                                });
                                routeNumber++;
                            }
                        }
                        else
                        {
                            allRoutes.Add(new RouteInfo
                            {
                                RoomName = $"{roomName} (Origem {origin})",
                                RouteDetails = "Nenhuma rota encontrada."
                            });
                        }
                    }
                    else
                    {
                        allRoutes.Add(new RouteInfo
                        {
                            RoomName = $"{roomName} (Origem {origin})",
                            RouteDetails = "Erro ao obter rotas."
                        });
                    }
                }
                catch (Exception ex)
                {
                    allRoutes.Add(new RouteInfo
                    {
                        RoomName = $"{roomName} (Origem {origin})",
                        RouteDetails = $"Exceção: {ex.Message}"
                    });
                }
            }
            return allRoutes;
        }

        public class RouteInfo
        {
            public string RoomName { get; set; }
            public string RouteDetails { get; set; }
        }
    }
}
