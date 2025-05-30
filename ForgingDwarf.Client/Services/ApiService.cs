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
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5000") };

        // Аутентификация
        public async Task<bool> RegisterAsync(Common.Models.Client client)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", client);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginAsync(string name, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Name = name, Password = password });
            return response.IsSuccessStatusCode;
        }

        // Остальные методы (CRUD)...
    }
}
