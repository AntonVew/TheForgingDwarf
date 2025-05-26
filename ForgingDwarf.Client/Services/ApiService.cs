using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ForgingDwarf.Common.Models;

namespace ForgingDwarf.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        public async Task<List<Order>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/orders");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Order>>();
        }

        public async Task<List<Common.Models.Client>> GetClientsAsync()
        {
            var response = await _httpClient.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Common.Models.Client>>();
        }

        public async Task CreateOrderAsync(Order order)
        {
            var response = await _httpClient.PostAsJsonAsync("api/orders", order);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateClientAsync(Common.Models.Client client)
        {
            var response = await _httpClient.PostAsJsonAsync("api/clients", client);
            response.EnsureSuccessStatusCode();
        }
    }
}
