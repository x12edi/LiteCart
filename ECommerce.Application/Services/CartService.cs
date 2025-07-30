using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartDto> GetByIdAsync(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
                return null;

            var cartItems = await _unitOfWork.CartItems.GetAllAsync();
            var items = cartItems.Where(ci => ci.CartId == id)
                .Select(ci => new CartItemDto
                {
                    CartId = ci.CartId,
                    ProductVariantId = ci.ProductVariantId,
                    Quantity = ci.Quantity,
                    PriceAtTime = ci.PriceAtTime
                });

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                SessionId = cart.SessionId,
                Items = items
            };
        }

        public async Task<CartDto> GetByUserIdOrSessionAsync(int? userId, string sessionId)
        {
            Cart cart = null;
            if (userId.HasValue)
            {
                var carts = await _unitOfWork.Carts.GetAllAsync();
                cart = carts.FirstOrDefault(c => c.UserId == userId);
            }
            if (cart == null && !string.IsNullOrEmpty(sessionId))
            {
                var carts = await _unitOfWork.Carts.GetAllAsync();
                cart = carts.FirstOrDefault(c => c.SessionId == sessionId);
            }
            if (cart == null)
                return null;

            var cartItems = await _unitOfWork.CartItems.GetAllAsync();
            var items = cartItems.Where(ci => ci.CartId == cart.Id)
                .Select(ci => new CartItemDto
                {
                    CartId = ci.CartId,
                    ProductVariantId = ci.ProductVariantId,
                    Quantity = ci.Quantity,
                    PriceAtTime = ci.PriceAtTime
                });

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                SessionId = cart.SessionId,
                Items = items
            };
        }

        public async Task<CartDto> CreateAsync(CreateCartDto dto)
        {
            if (string.IsNullOrEmpty(dto.SessionId))
                throw new ArgumentException("SessionId is required.");

            var cart = new Cart
            {
                UserId = dto.UserId,
                SessionId = dto.SessionId
            };

            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.CompleteAsync();

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                SessionId = cart.SessionId,
                Items = Enumerable.Empty<CartItemDto>()
            };
        }

        public async Task AddItemAsync(int cartId, AddCartItemDto dto)
        {
            if (dto.Quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            var cart = await _unitOfWork.Carts.GetByIdAsync(cartId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(dto.ProductVariantId);
            if (variant == null)
                throw new KeyNotFoundException($"Product variant {dto.ProductVariantId} not found.");
            if (variant.Stock < dto.Quantity)
                throw new InvalidOperationException($"Insufficient stock for product variant {dto.ProductVariantId}.");

            var existingItem = await _unitOfWork.CartItems.GetByCartAndProductVariantIdAsync(cartId, dto.ProductVariantId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                existingItem.PriceAtTime = variant.Price;
                await _unitOfWork.CartItems.UpdateAsync(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductVariantId = dto.ProductVariantId,
                    Quantity = dto.Quantity,
                    PriceAtTime = variant.Price
                };
                await _unitOfWork.CartItems.AddAsync(cartItem);
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateItemAsync(int cartId, int productVariantId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            var cart = await _unitOfWork.Carts.GetByIdAsync(cartId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var cartItem = await _unitOfWork.CartItems.GetByCartAndProductVariantIdAsync(cartId, productVariantId);
            if (cartItem == null)
                throw new KeyNotFoundException($"Cart item not found.");

            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(productVariantId);
            if (variant == null)
                throw new KeyNotFoundException($"Product variant {productVariantId} not found.");
            if (variant.Stock < quantity)
                throw new InvalidOperationException($"Insufficient stock for product variant {productVariantId}.");

            cartItem.Quantity = quantity;
            cartItem.PriceAtTime = variant.Price;
            await _unitOfWork.CartItems.UpdateAsync(cartItem);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveItemAsync(int cartId, int productVariantId)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(cartId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var cartItem = await _unitOfWork.CartItems.GetByCartAndProductVariantIdAsync(cartId, productVariantId);
            if (cartItem == null)
                throw new KeyNotFoundException($"Cart item not found.");

            await _unitOfWork.CartItems.DeleteByCartAndProductVariantIdAsync(cartId, productVariantId);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var cartItems = await _unitOfWork.CartItems.GetAllAsync();
            foreach (var item in cartItems.Where(ci => ci.CartId == id))
            {
                await _unitOfWork.CartItems.DeleteByCartAndProductVariantIdAsync(id, item.ProductVariantId);
            }

            await _unitOfWork.Carts.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}