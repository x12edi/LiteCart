using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class ShippingMethodRepository : IRepository<ShippingMethod>
    {
        private readonly string _connectionString;

        public ShippingMethodRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ShippingMethod> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ShippingMethods WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ShippingMethod
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Cost = reader.GetDecimal("Cost"),
                    EstimatedTime = reader.GetString("EstimatedTime"),
                    AvailabilityRegion = reader.GetString("AvailabilityRegion")
                };
            }
            return null;
        }

        public async Task<IEnumerable<ShippingMethod>> GetAllAsync()
        {
            var shippingMethods = new List<ShippingMethod>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ShippingMethods", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                shippingMethods.Add(new ShippingMethod
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Cost = reader.GetDecimal("Cost"),
                    EstimatedTime = reader.GetString("EstimatedTime"),
                    AvailabilityRegion = reader.GetString("AvailabilityRegion")
                });
            }
            return shippingMethods;
        }

        public async Task AddAsync(ShippingMethod entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO ShippingMethods (Name, Cost, EstimatedTime, AvailabilityRegion) " +
                "VALUES (@Name, @Cost, @EstimatedTime, @AvailabilityRegion)", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Cost", entity.Cost);
            command.Parameters.AddWithValue("@EstimatedTime", entity.EstimatedTime);
            command.Parameters.AddWithValue("@AvailabilityRegion", entity.AvailabilityRegion);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(ShippingMethod entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE ShippingMethods SET Name = @Name, Cost = @Cost, EstimatedTime = @EstimatedTime, " +
                "AvailabilityRegion = @AvailabilityRegion WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Cost", entity.Cost);
            command.Parameters.AddWithValue("@EstimatedTime", entity.EstimatedTime);
            command.Parameters.AddWithValue("@AvailabilityRegion", entity.AvailabilityRegion);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM ShippingMethods WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}