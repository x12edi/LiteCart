using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Categories WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Category
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    ParentId = reader["ParentId"] != DBNull.Value ? reader.GetInt32("ParentId") : null
                };
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = new List<Category>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Categories", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    ParentId = reader["ParentId"] != DBNull.Value ? reader.GetInt32("ParentId") : null
                });
            }
            return categories;
        }

        public async Task AddAsync(Category entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO Categories (Name, ParentId) VALUES (@Name, @ParentId)", connection);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@ParentId", (object)entity.ParentId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE Categories SET Name = @Name, ParentId = @ParentId WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@ParentId", (object)entity.ParentId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("DELETE FROM Categories WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}