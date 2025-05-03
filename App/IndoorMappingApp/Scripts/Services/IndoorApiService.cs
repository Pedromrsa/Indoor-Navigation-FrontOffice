using IndoorMappingApp.Scripts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.Services
{
    internal class IndoorApiService
    {
        private readonly HttpClient _httpClient;

        public IndoorApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://isepindoornavigationapi-vgq7.onrender.com") // ⚠️ coloca aqui o endereço real da API
            };
        }

        public async Task<GetMelhorCaminhoDTO?> ObterMelhorCaminhoAsync(long destinoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Caminhos/melhor-caminho?destinoId={destinoId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<GetMelhorCaminhoDTO>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter caminho: {ex.Message}");
            }

            return null;
        }

        public async Task<List<GetInfraestruturaDTO>> GetInfraestruturasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/infraestrutura");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<GetInfraestruturaDTO>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<GetInfraestruturaDTO>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter infraestruturas: {ex.Message}");
            }

            return new List<GetInfraestruturaDTO>();
        }
        public async Task<RegisterResponseDTO?> RegisterAsync(RegisterRequestDTO dto)
        {
            try
            {
                // POST /api/Auth/register2 with JSON body
                var response = await _httpClient.PostAsJsonAsync("api/Auth/register2",dto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RegisterResponseDTO>();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Registration failed: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
            }

            return null;
        }

        public async Task<RegisterResponseDTO> UpdateAccountSettingsAsync(UpdateAccountRequestDTO dto)
        {
            var client = new HttpClient();
            var url = "https://isepindoornavigationapi-vgq7.onrender.com/api/Account/update";

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RegisterResponseDTO>(responseBody);
            }
            catch (Exception ex)
            {
                return new RegisterResponseDTO { Success = false, Message = ex.Message };
            }
        }

        public async Task<ApiResponseDTO> UpdateAccountSettingsAsync(UpdateAccountRequestDTO dto)
        {
            var client = new HttpClient();
            var url = "https://isepindoornavigationapi-vgq7.onrender.com/api/Account/update"; // adjust if different

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponseDTO>(responseBody);
            }
            catch (Exception ex)
            {
                return new ApiResponseDTO { Success = false, Message = ex.Message };
            }
        }
    }
}
