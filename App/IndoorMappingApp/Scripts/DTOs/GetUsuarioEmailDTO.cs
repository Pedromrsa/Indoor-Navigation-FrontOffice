using System.Text.Json.Serialization;

namespace IndoorMappingApp.Scripts.DTOs
{
    public class GetUsuarioEmailDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
