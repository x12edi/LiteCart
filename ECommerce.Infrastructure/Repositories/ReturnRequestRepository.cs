using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class ReturnRequestRepository : IRepository<ReturnRequest>
    {
        private readonly string _connectionString;

        public ReturnRequestRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ReturnRequest> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ReturnRequests WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ReturnRequest
                {
                    Id = reader.GetInt32("Id"),
                    OrderItemId = reader.GetInt32("OrderItemId"),
                    Reason = reader.GetString("Reason"),
                    Status = reader.GetString("Status"),
                    ResolutionType = reader.GetString("ResolutionType")
                };
            }
            return null;
        }

        public async Task<IEnumerable<ReturnRequest>> GetAllAsync()
        {
            var returnRequests = new List<ReturnRequest>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ReturnRequests", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                returnRequests.Add(new ReturnRequest
                {
                    Id = reader.GetInt32("Id"),
                    OrderItemId = reader.GetInt32("OrderItemId"),
                    Reason = reader.GetString("Reason"),
                    Status = reader.GetString("Status"),
                    ResolutionType = reader.GetString("ResolutionType")
                });
            }
            return returnRequests;
        }

        public async Task AddAsync(ReturnRequest entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO ReturnRequests (OrderItemId, Reason, Status, ResolutionType) " +
                "VALUES (@OrderItemId, @Reason, @Status, @ResolutionType)", connection);
            command.Parameters.AddWithValue("@OrderItemId", entity.OrderItemId);
            command.Parameters.AddWithValue("@Reason", entity.Reason);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@ResolutionType", entity.ResolutionType);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(ReturnRequest entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE ReturnRequests SET OrderItemId = @OrderItemId, Reason = @Reason, " +
                "Status = @Status, ResolutionType = @ResolutionType WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@OrderItemId", entity.OrderItemId);
            command.Parameters.AddWithValue("@Reason", entity.Reason);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@ResolutionType", entity.ResolutionType);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM ReturnRequests WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}