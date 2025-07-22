using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class InventoryRepository : IRepository<Inventory>
    {
        private readonly string _connectionString;

        public InventoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Inventory> GetByIdAsync(int id)
        {
            throw new NotSupportedException("GetById is not supported for Inventory. Use GetByProductVariantAndWarehouseId instead.");
        }

        public async Task<Inventory> GetByProductVariantAndWarehouseIdAsync(int productId, int? variantId, int warehouseId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT * FROM Inventories WHERE ProductId = @ProductId AND VariantId = @VariantId AND WarehouseId = @WarehouseId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@VariantId", (object)variantId ?? DBNull.Value);
            command.Parameters.AddWithValue("@WarehouseId", warehouseId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Inventory
                {
                    ProductId = reader.GetInt32("ProductId"),
                    VariantId = reader["VariantId"] != DBNull.Value ? reader.GetInt32("VariantId") : null,
                    Quantity = reader.GetInt32("Quantity"),
                    WarehouseId = reader.GetInt32("WarehouseId")
                };
            }
            return null;
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            var inventories = new List<Inventory>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Inventories", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                inventories.Add(new Inventory
                {
                    ProductId = reader.GetInt32("ProductId"),
                    VariantId = reader["VariantId"] != DBNull.Value ? reader.GetInt32("VariantId") : null,
                    Quantity = reader.GetInt32("Quantity"),
                    WarehouseId = reader.GetInt32("WarehouseId")
                });
            }
            return inventories;
        }

        public async Task AddAsync(Inventory entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Inventories (ProductId, VariantId, Quantity, WarehouseId) " +
                "VALUES (@ProductId, @VariantId, @Quantity, @WarehouseId)", connection);
            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@VariantId", (object)entity.VariantId ?? DBNull.Value);
            command.Parameters.AddWithValue("@Quantity", entity.Quantity);
            command.Parameters.AddWithValue("@WarehouseId", entity.WarehouseId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Inventory entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Inventories SET Quantity = @Quantity WHERE ProductId = @ProductId AND VariantId = @VariantId AND WarehouseId = @WarehouseId", connection);
            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@VariantId", (object)entity.VariantId ?? DBNull.Value);
            command.Parameters.AddWithValue("@Quantity", entity.Quantity);
            command.Parameters.AddWithValue("@WarehouseId", entity.WarehouseId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotSupportedException("Delete by single Id is not supported for Inventory. Use DeleteByProductVariantAndWarehouseId instead.");
        }

        public async Task DeleteByProductVariantAndWarehouseIdAsync(int productId, int? variantId, int warehouseId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "DELETE FROM Inventories WHERE ProductId = @ProductId AND VariantId = @VariantId AND WarehouseId = @WarehouseId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@VariantId", (object)variantId ?? DBNull.Value);
            command.Parameters.AddWithValue("@WarehouseId", warehouseId);

            await command.ExecuteNonQueryAsync();
        }
    }
}