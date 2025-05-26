using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ForgingDwarf.Common.Models;

namespace ForgingDwarf.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000/api/orders";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            return await response.Content.ReadAsAsync<List<Order>>();
        }
    }
}
