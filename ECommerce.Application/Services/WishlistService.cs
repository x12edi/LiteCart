using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WishlistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WishlistDto> GetByKeysAsync(int userId, int productId)
        {
            var wishlist = await _unitOfWork.Wishlists.GetByUserAndProductIdAsync(userId, productId);
            if (wishlist == null)
                return null;

            return new WishlistDto
            {
                UserId = wishlist.UserId,
                ProductId = wishlist.ProductId
            };
        }

        public async Task<IEnumerable<WishlistDto>> GetAllAsync()
        {
            var wishlists = await _unitOfWork.Wishlists.GetAllAsync();
            return wishlists.Select(w => new WishlistDto
            {
                UserId = w.UserId,
                ProductId = w.ProductId
            });
        }

        public async Task<WishlistDto> CreateAsync(CreateWishlistDto dto)
        {
            if (dto.UserId <= 0 || dto.ProductId <= 0)
                throw new ArgumentException("Valid UserId and ProductId are required.");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            var wishlist = new Wishlist
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId
            };

            await _unitOfWork.Wishlists.AddAsync(wishlist);
            await _unitOfWork.CompleteAsync();

            return new WishlistDto
            {
                UserId = wishlist.UserId,
                ProductId = wishlist.ProductId
            };
        }

        public async Task DeleteAsync(int userId, int productId)
        {
            var wishlist = await _unitOfWork.Wishlists.GetByUserAndProductIdAsync(userId, productId);
            if (wishlist == null)
                throw new KeyNotFoundException("Wishlist item not found.");

            await _unitOfWork.Wishlists.DeleteByUserAndProductIdAsync(userId, productId);
            await _unitOfWork.CompleteAsync();
        }
    }
}