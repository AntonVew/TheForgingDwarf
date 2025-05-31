using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ForgingDwarf.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5000") };

        // Аутентификация
        public async Task<bool> RegisterAsync(Common.Models.Client client)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(client),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("api/auth/register", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка сервера: {error}"); // Лог ошибки
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginAsync(string name, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Name = name, Password = password });
            return response.IsSuccessStatusCode;
        }



        // Остальные методы (CRUD)...
    }
}
