using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class TaxRuleRepository : IRepository<TaxRule>
    {
        private readonly string _connectionString;

        public TaxRuleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<TaxRule> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM TaxRules WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TaxRule
                {
                    Id = reader.GetInt32("Id"),
                    Region = reader.GetString("Region"),
                    Rate = reader.GetDecimal("Rate"),
                    Type = reader.GetString("Type")
                };
            }
            return null;
        }

        public async Task<IEnumerable<TaxRule>> GetAllAsync()
        {
            var taxRules = new List<TaxRule>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM TaxRules", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                taxRules.Add(new TaxRule
                {
                    Id = reader.GetInt32("Id"),
                    Region = reader.GetString("Region"),
                    Rate = reader.GetDecimal("Rate"),
                    Type = reader.GetString("Type")
                });
            }
            return taxRules;
        }

        public async Task AddAsync(TaxRule entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO TaxRules (Region, Rate, Type) VALUES (@Region, @Rate, @Type)", connection);
            command.Parameters.AddWithValue("@Region", entity.Region);
            command.Parameters.AddWithValue("@Rate", entity.Rate);
            command.Parameters.AddWithValue("@Type", entity.Type);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(TaxRule entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE TaxRules SET Region = @Region, Rate = @Rate, Type = @Type WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Region", entity.Region);
            command.Parameters.AddWithValue("@Rate", entity.Rate);
            command.Parameters.AddWithValue("@Type", entity.Type);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM TaxRules WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}