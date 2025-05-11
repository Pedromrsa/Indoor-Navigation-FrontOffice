using IndoorMappingApp.Scripts.DTOs;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
                var response = await _httpClient.GetAsync("api/infraestrutura/GetAll");

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

        public async Task<HttpResponseMessage?> RegisterAsync(RegisterRequestDTO dto)
        {
            HttpResponseMessage response = new();
            try
            {
                response = await _httpClient.PostAsJsonAsync("api/Auth/register2", dto);
                // POST /api/Auth/register2 with JSON body
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Registration failed: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
            }

            return response;
        }

        public async Task<RegisterResponseDTO> UpdateUserInfoAsync(int id, UpdateAccountRequestDTO dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Usuarios/{id}", content);

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return new RegisterResponseDTO
                    {
                        Success = true,
                        Message = "Settings updated successfully."
                    };
                }

                var responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    return new RegisterResponseDTO
                    {
                        Success = false,
                        Message = "No content returned from server."
                    };
                }

                return JsonSerializer.Deserialize<RegisterResponseDTO>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
            }
            catch (Exception ex)
            {
                return new RegisterResponseDTO
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        //public async Task<bool> RequestRecoveryTokenAsync(string email)
        //{
        //    try
        //    {
        //        var payload = new ValidateTokenResponseDTO
        //        {
        //            Message = email
        //        };

        //        var response = await _httpClient.PostAsJsonAsync("api/Auth/recover", payload);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in RequestRecoveryTokenAsync: {ex.Message}");
        //        return false;
        //    }
        //}

        public async Task<bool> ValidateEmailExistance(string email)
        {
            try
            {

                HttpResponseMessage response = await _httpClient.PostAsync($"api/Auth/request-recovery-token?email={Uri.EscapeDataString(email)}", null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on Validating data: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> ValidateRecoveryTokenAsync(string token)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Auth/validate-token?token={token}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateRecoveryTokenAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            try
            {
                var payload = new { Token = token, NewPassword = newPassword };
                var response = await _httpClient.PostAsJsonAsync("api/Auth/reset-password", payload);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ResetPasswordAsync: {ex.Message}");
                return false;
            }
        }

    }
}
