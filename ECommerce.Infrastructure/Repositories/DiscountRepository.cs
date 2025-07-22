using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class DiscountRepository : IRepository<Discount>
    {
        private readonly string _connectionString;

        public DiscountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Discount> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Discounts WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Discount
                {
                    Id = reader.GetInt32("Id"),
                    Code = reader.GetString("Code"),
                    DiscountType = reader.GetString("DiscountType"),
                    Value = reader.GetDecimal("Value"),
                    Expiration = reader["Expiration"] != DBNull.Value ? reader.GetDateTime("Expiration") : null,
                    UsageLimit = reader["UsageLimit"] != DBNull.Value ? reader.GetInt32("UsageLimit") : null,
                    ApplicableProductId = reader["ApplicableProductId"] != DBNull.Value ? reader.GetInt32("ApplicableProductId") : null,
                    ApplicableCategoryId = reader["ApplicableCategoryId"] != DBNull.Value ? reader.GetInt32("ApplicableCategoryId") : null
                };
            }
            return null;
        }

        public async Task<IEnumerable<Discount>> GetAllAsync()
        {
            var discounts = new List<Discount>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Discounts", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                discounts.Add(new Discount
                {
                    Id = reader.GetInt32("Id"),
                    Code = reader.GetString("Code"),
                    DiscountType = reader.GetString("DiscountType"),
                    Value = reader.GetDecimal("Value"),
                    Expiration = reader["Expiration"] != DBNull.Value ? reader.GetDateTime("Expiration") : null,
                    UsageLimit = reader["UsageLimit"] != DBNull.Value ? reader.GetInt32("UsageLimit") : null,
                    ApplicableProductId = reader["ApplicableProductId"] != DBNull.Value ? reader.GetInt32("ApplicableProductId") : null,
                    ApplicableCategoryId = reader["ApplicableCategoryId"] != DBNull.Value ? reader.GetInt32("ApplicableCategoryId") : null
                });
            }
            return discounts;
        }

        public async Task AddAsync(Discount entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Discounts (Code, DiscountType, Value, Expiration, UsageLimit, ApplicableProductId, ApplicableCategoryId) " +
                "VALUES (@Code, @DiscountType, @Value, @Expiration, @UsageLimit, @ApplicableProductId, @ApplicableCategoryId)", connection);
            command.Parameters.AddWithValue("@Code", entity.Code);
            command.Parameters.AddWithValue("@DiscountType", entity.DiscountType);
            command.Parameters.AddWithValue("@Value", entity.Value);
            command.Parameters.AddWithValue("@Expiration", (object)entity.Expiration ?? DBNull.Value);
            command.Parameters.AddWithValue("@UsageLimit", (object)entity.UsageLimit ?? DBNull.Value);
            command.Parameters.AddWithValue("@ApplicableProductId", (object)entity.ApplicableProductId ?? DBNull.Value);
            command.Parameters.AddWithValue("@ApplicableCategoryId", (object)entity.ApplicableCategoryId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Discount entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Discounts SET Code = @Code, DiscountType = @DiscountType, Value = @Value, Expiration = @Expiration, " +
                "UsageLimit = @UsageLimit, ApplicableProductId = @ApplicableProductId, ApplicableCategoryId = @ApplicableCategoryId WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Code", entity.Code);
            command.Parameters.AddWithValue("@DiscountType", entity.DiscountType);
            command.Parameters.AddWithValue("@Value", entity.Value);
            command.Parameters.AddWithValue("@Expiration", (object)entity.Expiration ?? DBNull.Value);
            command.Parameters.AddWithValue("@UsageLimit", (object)entity.UsageLimit ?? DBNull.Value);
            command.Parameters.AddWithValue("@ApplicableProductId", (object)entity.ApplicableProductId ?? DBNull.Value);
            command.Parameters.AddWithValue("@ApplicableCategoryId", (object)entity.ApplicableCategoryId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Discounts WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}