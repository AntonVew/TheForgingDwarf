using ForgingDwarf.Server.Controllers;
using ForgingDwarf.Server.Repositories;
using ForgingDwarf.Server.Data;
using ForgingDwarf.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ForgingDwarf.Server.Tests
{
    public class OrdersControllerDeleteTests : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;
        private readonly AppDbContext _dbContext;
        private readonly Mock<OrderRepository> _orderRepositoryMock;

        public OrdersControllerDeleteTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Db_" + Guid.NewGuid())
                .Options;

            _dbContext = new AppDbContext(_dbOptions);

            _orderRepositoryMock = new Mock<OrderRepository>(_dbContext);
        }

        [Fact] //удаление существующего заказа
        public async Task Delete_ExistingOrder_ReturnsNoContent()
        {
            var testOrder = new Order { Id = 1 };
            _dbContext.Orders.Add(testOrder);
            await _dbContext.SaveChangesAsync();

            var controller = new OrdersController(_dbContext, _orderRepositoryMock.Object);

            var result = await controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _dbContext.Orders.FindAsync(1));
        }

        [Fact] //удаление несуществующего заказа
        public async Task Delete_NonExistingOrder_ReturnsNotFound()
        {
            var controller = new OrdersController(_dbContext, _orderRepositoryMock.Object);

            var result = await controller.Delete(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact] //проверка удаления
        public async Task Delete_Order_RemovesOrderButKeepsClient()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Delete_Test_Db")
                .Options;

            var client = new Client { Id = 1, Name = "Test Client", Password = "123" };
            var order = new Order { Id = 1, Client = client };

            using (var arrangeContext = new AppDbContext(options))
            {
                arrangeContext.Clients.Add(client);
                arrangeContext.Orders.Add(order);
                await arrangeContext.SaveChangesAsync();
            }

            using (var actContext = new AppDbContext(options))
            {
                var repository = new OrderRepository(actContext);
                var controller = new OrdersController(actContext, repository);
                var result = await controller.Delete(1);
            }

            using (var assertContext = new AppDbContext(options))
            {
                Assert.Null(await assertContext.Orders.FindAsync(1));

                var remainingClient = await assertContext.Clients.FindAsync(1);
                Assert.NotNull(remainingClient);
                Assert.Equal("Test Client", remainingClient.Name);

                Assert.Empty(assertContext.Orders.Where(o => o.ClientId == 1));
            }
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}