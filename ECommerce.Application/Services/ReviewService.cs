using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReviewDto> GetByKeysAsync(int productId, int userId)
        {
            var review = await _unitOfWork.Reviews.GetByProductAndUserIdAsync(productId, userId);
            if (review == null)
                return null;

            return new ReviewDto
            {
                ProductId = review.ProductId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            return reviews.Select(r => new ReviewDto
            {
                ProductId = r.ProductId,
                UserId = r.UserId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            });
        }

        public async Task<ReviewDto> CreateAsync(CreateReviewDto dto)
        {
            if (dto.ProductId <= 0 || dto.UserId <= 0)
                throw new ArgumentException("Valid ProductId and UserId are required.");
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var review = new Review
            {
                ProductId = dto.ProductId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.CompleteAsync();

            return new ReviewDto
            {
                ProductId = review.ProductId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task UpdateAsync(int productId, int userId, UpdateReviewDto dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var review = await _unitOfWork.Reviews.GetByProductAndUserIdAsync(productId, userId);
            if (review == null)
                throw new KeyNotFoundException("Review not found.");

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            await _unitOfWork.Reviews.UpdateAsync(review);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int productId, int userId)
        {
            var review = await _unitOfWork.Reviews.GetByProductAndUserIdAsync(productId, userId);
            if (review == null)
                throw new KeyNotFoundException("Review not found.");

            await _unitOfWork.Reviews.DeleteByProductAndUserIdAsync(productId, userId);
            await _unitOfWork.CompleteAsync();
        }
    }
}