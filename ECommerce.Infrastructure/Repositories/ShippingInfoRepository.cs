using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class ShippingInfoRepository : IRepository<ShippingInfo>
    {
        private readonly string _connectionString;

        public ShippingInfoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ShippingInfo> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ShippingInfo WHERE OrderId = @OrderId", connection);
            command.Parameters.AddWithValue("@OrderId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ShippingInfo
                {
                    OrderId = reader.GetInt32("OrderId"),
                    AddressId = reader.GetInt32("AddressId"),
                    ShippingMethodId = reader.GetInt32("ShippingMethodId"),
                    TrackingNumber = reader["TrackingNumber"] != DBNull.Value ? reader.GetString("TrackingNumber") : null,
                    Status = reader.GetString("Status")
                };
            }
            return null;
        }

        public async Task<IEnumerable<ShippingInfo>> GetAllAsync()
        {
            var shippingInfos = new List<ShippingInfo>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ShippingInfo", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                shippingInfos.Add(new ShippingInfo
                {
                    OrderId = reader.GetInt32("OrderId"),
                    AddressId = reader.GetInt32("AddressId"),
                    ShippingMethodId = reader.GetInt32("ShippingMethodId"),
                    TrackingNumber = reader["TrackingNumber"] != DBNull.Value ? reader.GetString("TrackingNumber") : null,
                    Status = reader.GetString("Status")
                });
            }
            return shippingInfos;
        }

        public async Task AddAsync(ShippingInfo entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO ShippingInfo (OrderId, AddressId, ShippingMethodId, TrackingNumber, Status) " +
                "VALUES (@OrderId, @AddressId, @ShippingMethodId, @TrackingNumber, @Status)", connection);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);
            command.Parameters.AddWithValue("@AddressId", entity.AddressId);
            command.Parameters.AddWithValue("@ShippingMethodId", entity.ShippingMethodId);
            command.Parameters.AddWithValue("@TrackingNumber", (object)entity.TrackingNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", entity.Status);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(ShippingInfo entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE ShippingInfo SET AddressId = @AddressId, ShippingMethodId = @ShippingMethodId, " +
                "TrackingNumber = @TrackingNumber, Status = @Status WHERE OrderId = @OrderId", connection);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);
            command.Parameters.AddWithValue("@AddressId", entity.AddressId);
            command.Parameters.AddWithValue("@ShippingMethodId", entity.ShippingMethodId);
            command.Parameters.AddWithValue("@TrackingNumber", (object)entity.TrackingNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", entity.Status);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM ShippingInfo WHERE OrderId = @OrderId", connection);
            command.Parameters.AddWithValue("@OrderId", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}