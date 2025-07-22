using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class StockMovementRepository : IRepository<StockMovement>
    {
        private readonly string _connectionString;

        public StockMovementRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<StockMovement> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM StockMovements WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new StockMovement
                {
                    Id = reader.GetInt32("Id"),
                    ProductVariantId = reader.GetInt32("ProductVariantId"),
                    FromWarehouseId = reader["FromWarehouseId"] != DBNull.Value ? reader.GetInt32("FromWarehouseId") : null,
                    ToWarehouseId = reader["ToWarehouseId"] != DBNull.Value ? reader.GetInt32("ToWarehouseId") : null,
                    Quantity = reader.GetInt32("Quantity"),
                    Reason = reader.GetString("Reason"),
                    Timestamp = reader.GetDateTime("Timestamp")
                };
            }
            return null;
        }

        public async Task<IEnumerable<StockMovement>> GetAllAsync()
        {
            var stockMovements = new List<StockMovement>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM StockMovements", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                stockMovements.Add(new StockMovement
                {
                    Id = reader.GetInt32("Id"),
                    ProductVariantId = reader.GetInt32("ProductVariantId"),
                    FromWarehouseId = reader["FromWarehouseId"] != DBNull.Value ? reader.GetInt32("FromWarehouseId") : null,
                    ToWarehouseId = reader["ToWarehouseId"] != DBNull.Value ? reader.GetInt32("ToWarehouseId") : null,
                    Quantity = reader.GetInt32("Quantity"),
                    Reason = reader.GetString("Reason"),
                    Timestamp = reader.GetDateTime("Timestamp")
                });
            }
            return stockMovements;
        }

        public async Task AddAsync(StockMovement entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO StockMovements (ProductVariantId, FromWarehouseId, ToWarehouseId, Quantity, Reason, Timestamp) " +
                "VALUES (@ProductVariantId, @FromWarehouseId, @ToWarehouseId, @Quantity, @Reason, @Timestamp)", connection);
            command.Parameters.AddWithValue("@ProductVariantId", entity.ProductVariantId);
            command.Parameters.AddWithValue("@FromWarehouseId", (object)entity.FromWarehouseId ?? DBNull.Value);
            command.Parameters.AddWithValue("@ToWarehouseId", (object)entity.ToWarehouseId ?? DBNull.Value);
            command.Parameters.AddWithValue("@Quantity", entity.Quantity);
            command.Parameters.AddWithValue("@Reason", entity.Reason);
            command.Parameters.AddWithValue("@Timestamp", entity.Timestamp);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(StockMovement entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE StockMovements SET ProductVariantId = @ProductVariantId, FromWarehouseId = @FromWarehouseId, " +
                "ToWarehouseId = @ToWarehouseId, Quantity = @Quantity, Reason = @Reason, Timestamp = @Timestamp WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@ProductVariantId", entity.ProductVariantId);
            command.Parameters.AddWithValue("@FromWarehouseId", (object)entity.FromWarehouseId ?? DBNull.Value);
            command.Parameters.AddWithValue("@ToWarehouseId", (object)entity.ToWarehouseId ?? DBNull.Value);
            command.Parameters.AddWithValue("@Quantity", entity.Quantity);
            command.Parameters.AddWithValue("@Reason", entity.Reason);
            command.Parameters.AddWithValue("@Timestamp", entity.Timestamp);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM StockMovements WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}