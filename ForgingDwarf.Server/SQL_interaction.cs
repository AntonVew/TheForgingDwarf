using ForgingDwarf.Common.Models;
using Microsoft.Data.SqlClient;

namespace ForgingDwarf.Client
{
    class SQL_interaction
    {
        private readonly string _connectionString;

        public SQL_interaction(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> CreateOrderRawAsync(Order order)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
                INSERT INTO Orders (
                    ClientId, 
                    ItemType, 
                    SteelType, 
                    Style, 
                    OrderDate, 
                    IsCustom, 
                    Status, 
                    Price
                ) 
                VALUES (
                    @ClientId, 
                    @ItemType, 
                    @SteelType, 
                    @Style, 
                    @OrderDate, 
                    @IsCustom, 
                    @Status, 
                    @Price
                );
                SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ClientId", order.ClientId);
            command.Parameters.AddWithValue("@ItemType", order.ItemType.ToString());
            command.Parameters.AddWithValue("@SteelType", order.SteelType.ToString());
            command.Parameters.AddWithValue("@Style", order.Style.ToString());
            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            command.Parameters.AddWithValue("@IsCustom", order.IsCustom);
            command.Parameters.AddWithValue("@Status", order.Status.ToString());
            command.Parameters.AddWithValue("@Price", order.Price);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateOrderStatusRawAsync(int orderId, OrderStatus newStatus)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
                UPDATE Orders 
                SET Status = @Status 
                WHERE Id = @OrderId";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@OrderId", orderId);
            command.Parameters.AddWithValue("@Status", newStatus.ToString());

            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteOrderRawAsync(int orderId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = "DELETE FROM Orders WHERE Id = @OrderId";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@OrderId", orderId);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}
