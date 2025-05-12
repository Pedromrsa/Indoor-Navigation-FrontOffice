using System.Collections.ObjectModel;
using System.Text.Json;
using IndoorMappingApp.Scripts;
using IndoorMappingApp.Scripts.Services;
using IndoorMappingApp.Scripts.DTOs;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IndoorMappingApp
{
    public partial class AvailableRoutesPage : ContentPage
    {
        public ObservableCollection<RouteInfo> Routes { get; set; }
        private readonly HttpClient _httpClient = new HttpClient();
        private RouteInfo _selectedRoute;
        private List<DrawablePath> allDrawablePaths = new();
        private Dictionary<long, PointF> _infraIdToPixel = new();
        private readonly string _mapFileName = "mapa_isep.png";

        public string SelectedRouteText =>
            SelectedRoute == null
                ? "No route selected"
                : $"{SelectedRoute.RoomName}";
        public RouteInfo SelectedRoute
        {
            get => _selectedRoute;
            set
            {
                if (_selectedRoute != value)
                {
                    _selectedRoute = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedRouteText));
                }
            }
        }
        

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

            var api = new IndoorApiService();
            var infraestruturas = await api.GetInfraestruturasAsync();

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
                                List<long> ids = new List<long>();
                                foreach (var path in route)
                                {
                                    var X = infraestruturas.FirstOrDefault(x => x.Id == path.OrigemId);
                                    if (X != null)
                                    { 
                                        ids.Add(X.Id); 
                                    }
                                    
                                }
                                var lastInfra = infraestruturas.FirstOrDefault(x => x.Id == route.Last().DestinoId);
                                if (lastInfra != null)
                                {
                                    ids.Add(lastInfra.Id);
                                }
                                
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

                                var drawablePath = new DrawablePath
                                {
                                    InfraestruturasIds = ids,
                                    routeName = $"{roomName} (Origem {origem.OrigemTipoInfraestrutura} {origem.OrigemDescricao}, Rota {routeNumber})"
                                };
                                allDrawablePaths.Add(new DrawablePath
                                {
                                    InfraestruturasIds = ids,
                                    routeName = $"{roomName} (Origem {origem.OrigemTipoInfraestrutura} {origem.OrigemDescricao}, Rota {routeNumber})"
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
        private async void OnRouteSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedRoute = e.CurrentSelection[0] as RouteInfo;
                if (selectedRoute != null)
                {
                    SelectedRoute = selectedRoute;
                    // Handle the click here, e.g., show a popup, navigate, or highlight the route
                    // await DisplayAlert("Route Selected", selectedRoute.RoomName, "OK");
                    SelectedRouteLabel.Text = SelectedRoute.RoomName;
                    var routeToDraw = allDrawablePaths.FirstOrDefault(x => x.routeName.Equals(SelectedRoute.RoomName));
                    if (routeToDraw != null)
                    {
                        DrawRoute(routeToDraw);
                    }
                }
                // Clear selection to remove highlight
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public class RouteInfo
        {
            public string RoomName { get; set; }
            public string RouteDetails { get; set; }
        }

        private async void DrawRoute(DrawablePath resultado)
        {
            ImagemMapa.Source = _mapFileName;
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

        public class DrawablePath
        {
            public List<long> InfraestruturasIds;
            public string routeName;
        }
    }
}
