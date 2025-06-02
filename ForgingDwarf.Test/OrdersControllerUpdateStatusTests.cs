using ForgingDwarf.Server.Controllers;
using ForgingDwarf.Server.Repositories;
using ForgingDwarf.Server.Data;
using ForgingDwarf.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using static ForgingDwarf.Server.Controllers.OrdersController;

namespace ForgingDwarf.Test
{
    public class OrdersControllerUpdateStatusTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;
        private readonly AppDbContext _dbContext;
        private readonly Mock<OrderRepository> _orderRepositoryMock;

        public OrdersControllerUpdateStatusTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Db_" + Guid.NewGuid())
                .Options;

            _dbContext = new AppDbContext(_dbOptions);

            _orderRepositoryMock = new Mock<OrderRepository>(_dbContext);
        }

        [Fact] //успешное обновление статуса заказа
        public async Task UpdateStatus_ValidData_UpdatesStatus()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateStatus_Valid")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Orders.Add(new Order { Id = 1, Status = OrderStatus.InQueue });
                await context.SaveChangesAsync();
            }

            var dto = new StatusUpdateDto { Status = OrderStatus.InProgress };

            // Act
            using (var context = new AppDbContext(options))
            {
                var controller = new OrdersController(context, null);
                var result = await controller.UpdateStatus(1, dto);
            }

            // Assert
            using (var context = new AppDbContext(options))
            {
                var order = await context.Orders.FindAsync(1);
                Assert.Equal(OrderStatus.InProgress, order.Status);
            }
        }

        [Fact] //если укажем несуществующий заказ?
        public async Task UpdateStatus_OrderNotFound_ReturnsNotFound()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateStatus_NotFound")
                .Options;

            var dto = new StatusUpdateDto { Status = OrderStatus.Completed };

            using (var context = new AppDbContext(options))
            {
                var controller = new OrdersController(context, null);
                var result = await controller.UpdateStatus(999, dto);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact] //проверка на статус не из списка енамов
        public async Task UpdateStatus_InvalidStatus_ReturnsBadRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateStatus_Invalid_Test")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Orders.Add(new Order { Id = 1, Status = OrderStatus.InQueue });
                await context.SaveChangesAsync();
            }

            var invalidDto = new StatusUpdateDto
            {
                Status = (OrderStatus)999
            };

            using (var context = new AppDbContext(options))
            {
                var controller = new OrdersController(context, null);
                var result = await controller.UpdateStatus(1, invalidDto);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Недопустимый статус заказа", badRequestResult.Value);
            }
        }

    }
}
