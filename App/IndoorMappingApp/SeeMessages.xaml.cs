using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace IndoorMappingApp
{
    public partial class SeeMessages : ContentPage
    {
        private const string apiBaseUrl = "https://isepindoornavigationapi-vgq7.onrender.com/api/FeedbackCaminhos";
        private const string apiResponseUrl = "https://isepindoornavigationapi-vgq7.onrender.com/api/FeedbackForUser";

        // Observable collection for feedbacks with responses
        public ObservableCollection<FeedbackWithResponse> FeedbackWithResponses { get; set; }

        public SeeMessages()
        {
            InitializeComponent();
            FeedbackWithResponses = new ObservableCollection<FeedbackWithResponse>();
            GetFeedbacks(); // Load feedbacks for the user
        }

        // Method to get all feedbacks for the user
        private async void GetFeedbacks()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var feedbackResponse = await client.GetStringAsync(apiBaseUrl);

                    // Deserialize the JSON response to a list of Feedbacks
                    var feedbacks = JsonSerializer.Deserialize<List<Feedback>>(feedbackResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Fetch all admin responses
                    var adminResponse = await client.GetStringAsync(apiResponseUrl);

                    // Deserialize the JSON response to a list of AdminResponses
                    var adminResponses = JsonSerializer.Deserialize<List<AdminResponse>>(adminResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Check if feedbacks and admin responses are not null
                    if (feedbacks == null || adminResponses == null || !feedbacks.Any() || !adminResponses.Any())
                    {
                        await DisplayAlert("Erro", "Não foi possível carregar os dados.", "OK");
                        return;
                    }

                    // Filter feedbacks by UserId (string) and join with admin responses
                    int userId = int.Parse(ActiveUser.UserId); // ID of the user whose feedbacks you want to fetch
                    var feedbackWithResponses = (from feedback in feedbacks
                                                 where feedback.UsuarioId == userId
                                                 join adminRes in adminResponses on feedback.Id equals adminRes.FeedbackId into joinedResponses
                                                 from response in joinedResponses.DefaultIfEmpty()
                                                 where !string.IsNullOrWhiteSpace(feedback.Comentario)
                                                    && response != null
                                                    && !string.IsNullOrWhiteSpace(response.Comentario)
                                                 select new FeedbackWithResponse
                                                 {
                                                     Feedback = feedback,
                                                     AdminResponse = response
                                                 }).ToList();



                    foreach (var item in feedbackWithResponses)
                    {
                        FeedbackWithResponses.Add(item);
                    }

                    BindingContext = this;
                }
            }
            catch (Exception ex)
            {
                // Handle errors when loading the feedbacks
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                await DisplayAlert("Erro", $"Erro ao carregar os feedbacks: {ex.Message}", "OK");
            }
        }

        // Method for the burger menu button
        private async void OnBurgerMenuClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("options");
        }
    }

    // Model for Feedback
    public class Feedback
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }  // Changed to string
        public int CaminhoId { get; set; }
        public int Avaliacao { get; set; }
        public string Comentario { get; set; }
        public DateTime DataHora { get; set; }
    }

    // Model for Admin Response
    public class AdminResponse
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public int UsuarioId { get; set; }  // Changed to string
        public int AdminId { get; set; }
        public string Comentario { get; set; }
        public DateTime DataHora { get; set; }
    }

    // Model for grouping Feedback and Admin Response
    public class FeedbackWithResponse
    {
        public Feedback Feedback { get; set; }
        public AdminResponse AdminResponse { get; set; }
    }
}
