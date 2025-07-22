using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class PaymentRepository : IRepository<Payment>
    {
        private readonly string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Payments WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Payment
                {
                    Id = reader.GetInt32("Id"),
                    OrderId = reader.GetInt32("OrderId"),
                    Amount = reader.GetDecimal("Amount"),
                    Method = reader.GetString("Method"),
                    Status = reader.GetString("Status"),
                    TransactionId = reader["TransactionId"] != DBNull.Value ? reader.GetString("TransactionId") : null
                };
            }
            return null;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var payments = new List<Payment>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Payments", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                payments.Add(new Payment
                {
                    Id = reader.GetInt32("Id"),
                    OrderId = reader.GetInt32("OrderId"),
                    Amount = reader.GetDecimal("Amount"),
                    Method = reader.GetString("Method"),
                    Status = reader.GetString("Status"),
                    TransactionId = reader["TransactionId"] != DBNull.Value ? reader.GetString("TransactionId") : null
                });
            }
            return payments;
        }

        public async Task AddAsync(Payment entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Payments (OrderId, Amount, Method, Status, TransactionId) " +
                "VALUES (@OrderId, @Amount, @Method, @Status, @TransactionId)", connection);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);
            command.Parameters.AddWithValue("@Amount", entity.Amount);
            command.Parameters.AddWithValue("@Method", entity.Method);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@TransactionId", (object)entity.TransactionId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Payment entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Payments SET OrderId = @OrderId, Amount = @Amount, Method = @Method, " +
                "Status = @Status, TransactionId = @TransactionId WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@OrderId", entity.OrderId);
            command.Parameters.AddWithValue("@Amount", entity.Amount);
            command.Parameters.AddWithValue("@Method", entity.Method);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@TransactionId", (object)entity.TransactionId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Payments WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}