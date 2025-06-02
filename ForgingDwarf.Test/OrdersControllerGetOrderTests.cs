using ForgingDwarf.Server.Controllers;
using ForgingDwarf.Server.Repositories;
using ForgingDwarf.Server.Data;
using ForgingDwarf.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ForgingDwarf.Test
{
    public class OrdersControllerGetOrderTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;
        private readonly AppDbContext _dbContext;
        private readonly Mock<OrderRepository> _orderRepositoryMock;

        public OrdersControllerGetOrderTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Db_" + Guid.NewGuid())
                .Options;

            _dbContext = new AppDbContext(_dbOptions);

            _orderRepositoryMock = new Mock<OrderRepository>(_dbContext);
        }

        [Fact] //тест на получение заказа с клиентом
        public async Task GetOrder_WithClient_ReturnsOrderAndClient()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetOrder_WithClient")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var client = new Client
                {
                    Id = 1,
                    Name = "Test Client",
                    Password = "123456"
                };
                var order = new Order
                {
                    Id = 1,
                    ClientId = 1,
                    ItemType = ItemType.Weapon,
                    SteelType = SteelType.Damascus,
                    Style = Style.Gothic,
                    OrderDate = DateTime.Now,
                    IsCustom = true,
                    Status = OrderStatus.InQueue,
                    Price = 1000
                };

                context.Clients.Add(client);
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new OrdersController(context, null);

                var result = await controller.GetOrder(1);

                var actionResult = Assert.IsType<ActionResult<Order>>(result);
                var order = Assert.IsType<Order>(actionResult.Value);

                Assert.Equal(1, order.Id);
                Assert.Equal("Test Client", order.Client?.Name);
            }
        }

        [Fact] //тест на несуществующий заказ
        public async Task GetOrder_NotExists_ReturnsNotFound()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetOrderTest2")
                .Options;

            using var context = new AppDbContext(options);
            var controller = new OrdersController(context, null);

            var result = await controller.GetOrder(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact] //  корректная загрузка заказа вместе с клиентом
        public async Task GetOrder_WithClient_ReturnsClientName()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Client_Test_" + Guid.NewGuid())
                .Options;

            using (var context = new AppDbContext(options))
            {
                var client = new Client
                {
                    Id = 1,
                    Name = "Иван Иванов",
                    Password = "123456"
                };
                var order = new Order
                {
                    Id = 1,
                    ClientId = 1,
                    ItemType = ItemType.Weapon,
                    SteelType = SteelType.Damascus,
                    Style = Style.Gothic,
                    OrderDate = DateTime.Now,
                    IsCustom = false,
                    Status = OrderStatus.InQueue,
                    Price = 1000
                };

                context.Clients.Add(client);
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new OrdersController(context, null);
                var result = await controller.GetOrder(1);

                Assert.NotNull(result.Value);
                Assert.Equal("Иван Иванов", result.Value.Client?.Name);
                Assert.Equal(ItemType.Weapon, result.Value.ItemType);
            }
        }
    }
}
