using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using ForgingDwarf.Common.Models;
using static AuthController;

namespace ForgingDwarf.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5000") };

        // Аутентификация
        public async Task<RegistrationResult> RegisterAsync(ClientRegistrationDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

                if (response.IsSuccessStatusCode)
                    return RegistrationResult.Success();

                var errorContent = await response.Content.ReadAsStringAsync();
                return response.StatusCode switch
                {
                    HttpStatusCode.Conflict => RegistrationResult.Failure("Имя пользователя занято"),
                    HttpStatusCode.BadRequest => RegistrationResult.Failure("Некорректные данные"),
                    _ => RegistrationResult.Failure($"Ошибка: {errorContent}")
                };
            }
            catch (HttpRequestException ex)
            {
                return RegistrationResult.Failure($"Сетевая ошибка: {ex.Message}");
            }
        }

        public class RegistrationResult
        {
            public bool IsSuccess { get; }
            public string ErrorMessage { get; }

            private RegistrationResult(bool isSuccess, string errorMessage)
            {
                IsSuccess = isSuccess;
                ErrorMessage = errorMessage;
            }

            public static RegistrationResult Success() => new(true, null);
            public static RegistrationResult Failure(string error) => new(false, error);
        }

        public async Task<bool> LoginAsync(string name, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Name = name, Password = password });
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("api/orders");
        }

        public async Task<List<Common.Models.Client>> GetClientsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Common.Models.Client>>("api/clients");
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/orders", order);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка сервера: {response.StatusCode} - {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сетевая ошибка: {ex.Message}");
                return false;
            }
        }

        // Остальные методы (CRUD)...
    }
}
