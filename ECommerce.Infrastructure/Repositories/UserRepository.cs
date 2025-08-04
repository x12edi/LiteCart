using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    Phone = reader["Phone"] != DBNull.Value ? reader.GetString("Phone") : null,
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    IsActive = reader["IsActive"] != DBNull.Value ? reader.GetBoolean("IsActive") : false,
                    IsCustomer = reader["IsCustomer"] != DBNull.Value ? reader.GetBoolean("IsCustomer") : false,
                };
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = new List<User>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Users", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    Phone = reader["Phone"] != DBNull.Value ? reader.GetString("Phone") : null,
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    IsActive = reader["IsActive"] != DBNull.Value ? reader.GetBoolean("IsActive") : false,
                    IsCustomer = reader["IsCustomer"] != DBNull.Value ? reader.GetBoolean("IsCustomer") : false,
                });
            }
            return users;
        }

        public async Task AddAsync(User entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Users (Name, Email, PasswordHash, Phone, CreatedAt, IsActive, IsCustomer) " +
                "VALUES (@Name, @Email, @PasswordHash, @Phone, getutcdate(),@IsActive, @IsCustomer)", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
            command.Parameters.AddWithValue("@Phone", (object)entity.Phone ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", entity.IsActive);
            command.Parameters.AddWithValue("@IsCustomer", entity.IsCustomer);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Users SET Email = @Email, PasswordHash = @PasswordHash, Phone = @Phone, IsActive = @IsActive WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            //command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
            command.Parameters.AddWithValue("@Phone", (object)entity.Phone ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", entity.IsActive);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}