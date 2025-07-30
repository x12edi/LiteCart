using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IProductCategoriesRepository ProductCategories { get; }
        IRepository<ProductVariant> ProductVariants { get; }
        IInventoryRepository Inventories { get; }
        IRepository<User> Users { get; }
        IRepository<Cart> Carts { get; }
        ICartItemRepository CartItems { get; }        //IRepository<CartItem> CartItems { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Address> Addresses { get; }
        IRepository<ShippingInfo> ShippingInfos { get; }
        IReviewRepository Reviews { get; }
        IRepository<Role> Roles { get; }
        IUserRoleRepository UserRoles { get; }
        IRepository<Permission> Permissions { get; }
        IRolePermissionRepository RolePermissions { get; }
        IRepository<AdminLog> AdminLogs { get; }
        IRepository<Warehouse> Warehouses { get; }
        IRepository<StockMovement> StockMovements { get; }
        IRepository<Discount> Discounts { get; }
        IRepository<TaxRule> TaxRules { get; }
        IRepository<ShippingMethod> ShippingMethods { get; }
        IWishlistRepository Wishlists { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<ReturnRequest> ReturnRequests { get; }
        IRepository<CustomerSupportTicket> CustomerSupportTickets { get; }
        Task<int> CompleteAsync();
    }
}