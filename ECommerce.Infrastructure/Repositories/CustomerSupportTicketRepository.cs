using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class CustomerSupportTicketRepository : IRepository<CustomerSupportTicket>
    {
        private readonly string _connectionString;

        public CustomerSupportTicketRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<CustomerSupportTicket> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM CustomerSupportTickets WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CustomerSupportTicket
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Subject = reader.GetString("Subject"),
                    Description = reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    AssignedAdminId = reader["AssignedAdminId"] != DBNull.Value ? reader.GetInt32("AssignedAdminId") : null
                };
            }
            return null;
        }

        public async Task<IEnumerable<CustomerSupportTicket>> GetAllAsync()
        {
            var tickets = new List<CustomerSupportTicket>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM CustomerSupportTickets", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tickets.Add(new CustomerSupportTicket
                {
                    Id = reader.GetInt32("Id"),
                    UserId = reader.GetInt32("UserId"),
                    Subject = reader.GetString("Subject"),
                    Description = reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    AssignedAdminId = reader["AssignedAdminId"] != DBNull.Value ? reader.GetInt32("AssignedAdminId") : null
                });
            }
            return tickets;
        }

        public async Task AddAsync(CustomerSupportTicket entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO CustomerSupportTickets (UserId, Subject, Description, Status, AssignedAdminId) " +
                "VALUES (@UserId, @Subject, @Description, @Status, @AssignedAdminId)", connection);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Subject", entity.Subject);
            command.Parameters.AddWithValue("@Description", entity.Description);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@AssignedAdminId", (object)entity.AssignedAdminId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(CustomerSupportTicket entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE CustomerSupportTickets SET UserId = @UserId, Subject = @Subject, Description = @Description, " +
                "Status = @Status, AssignedAdminId = @AssignedAdminId WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Subject", entity.Subject);
            command.Parameters.AddWithValue("@Description", entity.Description);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@AssignedAdminId", (object)entity.AssignedAdminId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM CustomerSupportTickets WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}