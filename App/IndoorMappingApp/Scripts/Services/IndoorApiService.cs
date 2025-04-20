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
    }
}
