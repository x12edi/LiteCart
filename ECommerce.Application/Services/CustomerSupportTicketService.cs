using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CustomerSupportTicketService : ICustomerSupportTicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerSupportTicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerSupportTicketDto> GetByIdAsync(int id)
        {
            var ticket = await _unitOfWork.CustomerSupportTickets.GetByIdAsync(id);
            if (ticket == null)
                return null;

            return new CustomerSupportTicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                Subject = ticket.Subject,
                Description = ticket.Description,
                Status = ticket.Status,
                AssignedAdminId = ticket.AssignedAdminId
            };
        }

        public async Task<IEnumerable<CustomerSupportTicketDto>> GetAllAsync()
        {
            var tickets = await _unitOfWork.CustomerSupportTickets.GetAllAsync();
            return tickets.Select(t => new CustomerSupportTicketDto
            {
                Id = t.Id,
                UserId = t.UserId,
                Subject = t.Subject,
                Description = t.Description,
                Status = t.Status,
                AssignedAdminId = t.AssignedAdminId
            });
        }

        public async Task<CustomerSupportTicketDto> CreateAsync(CreateCustomerSupportTicketDto dto)
        {
            if (dto.UserId <= 0)
                throw new ArgumentException("Valid UserId is required.");
            if (string.IsNullOrWhiteSpace(dto.Subject) || string.IsNullOrWhiteSpace(dto.Description) ||
                string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Subject, Description, and Status are required.");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (dto.AssignedAdminId.HasValue)
            {
                var admin = await _unitOfWork.Users.GetByIdAsync(dto.AssignedAdminId.Value);
                if (admin == null)
                    throw new KeyNotFoundException("Assigned admin not found.");
            }

            var ticket = new CustomerSupportTicket
            {
                UserId = dto.UserId,
                Subject = dto.Subject,
                Description = dto.Description,
                Status = dto.Status,
                AssignedAdminId = dto.AssignedAdminId
            };

            await _unitOfWork.CustomerSupportTickets.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();

            return new CustomerSupportTicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                Subject = ticket.Subject,
                Description = ticket.Description,
                Status = ticket.Status,
                AssignedAdminId = ticket.AssignedAdminId
            };
        }

        public async Task UpdateAsync(int id, UpdateCustomerSupportTicketDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.");

            var ticket = await _unitOfWork.CustomerSupportTickets.GetByIdAsync(id);
            if (ticket == null)
                throw new KeyNotFoundException("Customer support ticket not found.");

            if (dto.AssignedAdminId.HasValue)
            {
                var admin = await _unitOfWork.Users.GetByIdAsync(dto.AssignedAdminId.Value);
                if (admin == null)
                    throw new KeyNotFoundException("Assigned admin not found.");
            }

            ticket.Status = dto.Status;
            ticket.AssignedAdminId = dto.AssignedAdminId;

            await _unitOfWork.CustomerSupportTickets.UpdateAsync(ticket);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _unitOfWork.CustomerSupportTickets.GetByIdAsync(id);
            if (ticket == null)
                throw new KeyNotFoundException("Customer support ticket not found.");

            await _unitOfWork.CustomerSupportTickets.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}