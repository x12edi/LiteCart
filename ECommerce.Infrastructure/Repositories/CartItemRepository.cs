using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class CartItemRepository : IRepository<CartItem>
    {
        private readonly string _connectionString;

        public CartItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            throw new NotSupportedException("GetById is not supported for CartItem. Use GetByCartAndProductVariantIdAsync instead.");
        }

        public async Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId)
        {
            var items = new List<CartItem>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM CartItems WHERE CartId = @CartId", connection);
            command.Parameters.AddWithValue("@CartId", cartId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new CartItem
                {
                    CartId = reader.GetInt32("CartId"),
                    ProductVariantId = reader.GetInt32("ProductVariantId"),
                    Quantity = reader.GetInt32("Quantity"),
                    PriceAtTime = reader.GetDecimal("PriceAtTime")
                });
            }
            return items;
        }


        public async Task<CartItem> GetByCartAndProductVariantIdAsync(int cartId, int productVariantId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "SELECT * FROM CartItems WHERE CartId = @CartId AND ProductVariantId = @ProductVariantId", connection);
            command.Parameters.AddWithValue("@CartId", cartId);
            command.Parameters.AddWithValue("@ProductVariantId", productVariantId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CartItem
                {
                    CartId = reader.GetInt32("CartId"),
                    ProductVariantId = reader.GetInt32("ProductVariantId"),
                    Quantity = reader.GetInt32("Quantity"),
                    PriceAtTime = reader.GetDecimal("PriceAtTime")
                };
            }
            return null;
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            var cartItems = new List<CartItem>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM CartItems", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                cartItems.Add(new CartItem
                {
                    CartId = reader.GetInt32("CartId"),
                    ProductVariantId = reader.GetInt32("ProductVariantId"),
                    Quantity = reader.GetInt32("Quantity"),
                    PriceAtTime = reader.GetDecimal("PriceAtTime")
                });
            }
            return cartItems;
        }

        public async Task AddAsync(CartItem entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "INSERT INTO CartItems (CartId, ProductVariantId, Quantity, PriceAtTime) " +
                "VALUES (@CartId, @ProductVariantId, @Quantity, @PriceAtTime)", connection);
            command.Parameters.AddWithValue("@CartId", entity.CartId);
            command.Parameters.AddWithValue("@ProductVariantId", entity.ProductVariantId);
            command.Parameters.AddWithValue("@Quantity", entity.Quantity);
            command.Parameters.AddWithValue("@PriceAtTime", entity.PriceAtTime);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(CartItem entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "UPDATE CartItems SET Quantity = @Quantity, PriceAtTime = @PriceAtTime " +
                "WHERE CartId = @CartId AND ProductVariantId = @ProductVariantId", connection);
            command.Parameters.AddWithValue("@CartId", entity.CartId);
            command.Parameters.AddWithValue("@ProductVariantId", entity.ProductVariantId);
            command.Parameters.AddWithValue("@Quantity", entity.Quantity);
            command.Parameters.AddWithValue("@PriceAtTime", entity.PriceAtTime);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotSupportedException("Delete by single Id is not supported for CartItem. Use DeleteByCartAndProductVariantIdAsync instead.");
        }

        public async Task DeleteByCartAndProductVariantIdAsync(int cartId, int productVariantId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand(
                "DELETE FROM CartItems WHERE CartId = @CartId AND ProductVariantId = @ProductVariantId", connection);
            command.Parameters.AddWithValue("@CartId", cartId);
            command.Parameters.AddWithValue("@ProductVariantId", productVariantId);

            await command.ExecuteNonQueryAsync();
        }
    }
}