using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<ProductCategories> ProductCategories { get; }
        IRepository<ProductVariant> ProductVariants { get; }
        IRepository<Inventory> Inventories { get; }
        IRepository<User> Users { get; }
        IRepository<Cart> Carts { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Address> Addresses { get; }
        IRepository<ShippingInfo> ShippingInfos { get; }
        IRepository<Review> Reviews { get; }
        IRepository<Role> Roles { get; }
        IRepository<UserRole> UserRoles { get; }
        IRepository<Permission> Permissions { get; }
        IRepository<RolePermission> RolePermissions { get; }
        IRepository<AdminLog> AdminLogs { get; }
        IRepository<Warehouse> Warehouses { get; }
        IRepository<StockMovement> StockMovements { get; }
        IRepository<Discount> Discounts { get; }
        IRepository<TaxRule> TaxRules { get; }
        IRepository<ShippingMethod> ShippingMethods { get; }
        IRepository<Wishlist> Wishlists { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<ReturnRequest> ReturnRequests { get; }
        IRepository<CustomerSupportTicket> CustomerSupportTickets { get; }
        Task<int> CompleteAsync();
    }
}