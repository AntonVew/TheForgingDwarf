using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Controllers;

namespace ForgingDwarf.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5000") };

        // Аутентификация
        public async Task<RegistrationResult> RegisterAsync(AuthController.ClientRegistrationDto request)
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

        //для главного окна (получение списка заказов)
        public async Task<List<OrderWithClient>> GetOrdersWithClientsAsync()
        {
            try
            {
                var orders = await _httpClient.GetFromJsonAsync<List<Order>>("api/orders");
                var clients = await _httpClient.GetFromJsonAsync<List<Common.Models.Client>>("api/clients");

                return orders.Join(
                    clients,
                    order => order.ClientId,
                    client => client.Id,
                    (order, client) => new OrderWithClient
                    {
                        Order = order,
                        ClientName = client.Name,
                        Id = order.Id,
                        ItemType = order.ItemType,
                        SteelType = order.SteelType,
                        Status = order.Status,
                        Style = order.Style,
                        OrderDate = order.OrderDate,
                        IsCustom = order.IsCustom,
                        Price = order.Price
                    }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных: {ex.Message}");
                return new List<OrderWithClient>();
            }
        }

        public class OrderWithClient
        {
            public Order Order { get; set; }
            public string ClientName { get; set; }
            public int Id { get; set; }
            public ItemType ItemType { get; set; }
            public SteelType SteelType { get; set; }
            public Style Style { get; set; }
            public DateTime OrderDate { get; set; }
            public bool IsCustom { get; set; }
            public OrderStatus Status { get; set; }
            public double Price { get; set; }
        }

        //Получаем клиентов
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

        //удаление заказа
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/orders/{orderId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка удаления: {ex.Message}");
                return false;
            }
        }

        //обновление статуса заказа
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(
                    $"api/orders/{orderId}/status",
                    new { Status = newStatus });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления статуса: {ex.Message}");
                return false;
            }
        }
    }
}
