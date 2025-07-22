using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly string _connectionString;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IRepository<Product> _products;
        private IRepository<Category> _categories;
        private IRepository<ProductCategories> _productCategories;
        private IRepository<ProductVariant> _productVariants;
        private IRepository<Inventory> _inventories;
        private IRepository<User> _users;
        private IRepository<Cart> _carts;
        private IRepository<CartItem> _cartItems;
        private IRepository<Order> _orders;
        private IRepository<OrderItem> _orderItems;
        private IRepository<Payment> _payments;
        private IRepository<Address> _addresses;
        private IRepository<ShippingInfo> _shippingInfos;
        private IRepository<Review> _reviews;
        private IRepository<Role> _roles;
        private IRepository<UserRole> _userRoles;
        private IRepository<Permission> _permissions;
        private IRepository<RolePermission> _rolePermissions;
        private IRepository<AdminLog> _adminLogs;
        private IRepository<Warehouse> _warehouses;
        private IRepository<StockMovement> _stockMovements;
        private IRepository<Discount> _discounts;
        private IRepository<TaxRule> _taxRules;
        private IRepository<ShippingMethod> _shippingMethods;
        private IRepository<Wishlist> _wishlists;
        private IRepository<Notification> _notifications;
        private IRepository<ReturnRequest> _returnRequests;
        private IRepository<CustomerSupportTicket> _customerSupportTickets;
        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IRepository<Product> Products => _products ??= new ProductRepository(_connectionString);
        public IRepository<Category> Categories => _categories ??= new CategoryRepository(_connectionString);
        public IRepository<ProductCategories> ProductCategories => _productCategories ??= new ProductCategoriesRepository(_connectionString);
        public IRepository<ProductVariant> ProductVariants => _productVariants ??= new ProductVariantRepository(_connectionString);
        public IRepository<Inventory> Inventories => _inventories ??= new InventoryRepository(_connectionString);
        public IRepository<User> Users => _users ??= new UserRepository(_connectionString);
        public IRepository<Cart> Carts => _carts ??= new CartRepository(_connectionString);
        public IRepository<CartItem> CartItems => _cartItems ??= new CartItemRepository(_connectionString);
        public IRepository<Order> Orders => _orders ??= new OrderRepository(_connectionString);
        public IRepository<OrderItem> OrderItems => _orderItems ??= new OrderItemRepository(_connectionString);
        public IRepository<Payment> Payments => _payments ??= new PaymentRepository(_connectionString);
        public IRepository<Address> Addresses => _addresses ??= new AddressRepository(_connectionString);
        public IRepository<ShippingInfo> ShippingInfos => _shippingInfos ??= new ShippingInfoRepository(_connectionString);
        public IRepository<Review> Reviews => _reviews ??= new ReviewRepository(_connectionString);
        public IRepository<Role> Roles => _roles ??= new RoleRepository(_connectionString);
        public IRepository<UserRole> UserRoles => _userRoles ??= new UserRoleRepository(_connectionString);
        public IRepository<Permission> Permissions => _permissions ??= new PermissionRepository(_connectionString);
        public IRepository<RolePermission> RolePermissions => _rolePermissions ??= new RolePermissionRepository(_connectionString);
        public IRepository<AdminLog> AdminLogs => _adminLogs ??= new AdminLogRepository(_connectionString);
        public IRepository<Warehouse> Warehouses => _warehouses ??= new WarehouseRepository(_connectionString);
        public IRepository<StockMovement> StockMovements => _stockMovements ??= new StockMovementRepository(_connectionString);
        public IRepository<Discount> Discounts => _discounts ??= new DiscountRepository(_connectionString);
        public IRepository<TaxRule> TaxRules => _taxRules ??= new TaxRuleRepository(_connectionString);
        public IRepository<ShippingMethod> ShippingMethods => _shippingMethods ??= new ShippingMethodRepository(_connectionString);
        public IRepository<Wishlist> Wishlists => _wishlists ??= new WishlistRepository(_connectionString);
        public IRepository<Notification> Notifications => _notifications ??= new NotificationRepository(_connectionString);
        public IRepository<ReturnRequest> ReturnRequests => _returnRequests ??= new ReturnRequestRepository(_connectionString);
        public IRepository<CustomerSupportTicket> CustomerSupportTickets => _customerSupportTickets ??= new CustomerSupportTicketRepository(_connectionString);

        public async Task<int> CompleteAsync()
        {
            try
            {
                _transaction?.Commit();
                return await Task.FromResult(1);
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = _connection.BeginTransaction();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}