using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ForgingDwarf.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _repository;

        public OrdersController(OrderRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAllOrders());
        }
    }
}
