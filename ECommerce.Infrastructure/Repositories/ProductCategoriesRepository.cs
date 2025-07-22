using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductCategoriesRepository : IRepository<ProductCategories>
    {
        private readonly string _connectionString;

        public ProductCategoriesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ProductCategories> GetByIdAsync(int id)
        {
            throw new NotSupportedException("GetById is not supported for ProductCategories. Use GetByProductAndCategoryId instead.");
        }

        public async Task<ProductCategories> GetByProductAndCategoryIdAsync(int productId, int categoryId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT * FROM ProductCategories WHERE ProductId = @ProductId AND CategoryId = @CategoryId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@CategoryId", categoryId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ProductCategories
                {
                    ProductId = reader.GetInt32("ProductId"),
                    CategoryId = reader.GetInt32("CategoryId")
                };
            }
            return null;
        }

        public async Task<IEnumerable<ProductCategories>> GetAllAsync()
        {
            var productCategories = new List<ProductCategories>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM ProductCategories", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                productCategories.Add(new ProductCategories
                {
                    ProductId = reader.GetInt32("ProductId"),
                    CategoryId = reader.GetInt32("CategoryId")
                });
            }
            return productCategories;
        }

        public async Task AddAsync(ProductCategories entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO ProductCategories (ProductId, CategoryId) VALUES (@ProductId, @CategoryId)", connection);
            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(ProductCategories entity)
        {
            throw new NotSupportedException("Update is not supported for ProductCategories. Delete and re-add instead.");
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotSupportedException("Delete by single Id is not supported for ProductCategories. Use DeleteByProductAndCategoryId instead.");
        }

        public async Task DeleteByProductAndCategoryIdAsync(int productId, int categoryId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "DELETE FROM ProductCategories WHERE ProductId = @ProductId AND CategoryId = @CategoryId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@CategoryId", categoryId);

            await command.ExecuteNonQueryAsync();
        }
    }
}