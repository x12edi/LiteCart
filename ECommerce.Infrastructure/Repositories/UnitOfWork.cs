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
        private IProductCategoriesRepository _productCategories;
        private IRepository<ProductVariant> _productVariants;
        private IInventoryRepository _inventories;
        private IRepository<User> _users;
        private IRepository<Cart> _carts;
        private ICartItemRepository _cartItems;
        private IRepository<Order> _orders;
        private IRepository<OrderItem> _orderItems;
        private IRepository<Payment> _payments;
        private IRepository<Address> _addresses;
        private IRepository<ShippingInfo> _shippingInfos;
        private IReviewRepository _reviews;
        private IRepository<Role> _roles;
        private IUserRoleRepository _userRoles;
        private IRepository<Permission> _permissions;
        private IRolePermissionRepository _rolePermissions;
        private IRepository<AdminLog> _adminLogs;
        private IRepository<Warehouse> _warehouses;
        private IRepository<StockMovement> _stockMovements;
        private IRepository<Discount> _discounts;
        private IRepository<TaxRule> _taxRules;
        private IRepository<ShippingMethod> _shippingMethods;
        private IWishlistRepository _wishlists;
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
        public IProductCategoriesRepository ProductCategories => _productCategories ??= new ProductCategoriesRepository(_connectionString);
        public IRepository<ProductVariant> ProductVariants => _productVariants ??= new ProductVariantRepository(_connectionString);
        public IInventoryRepository Inventories => _inventories ??= (IInventoryRepository) new InventoryRepository(_connectionString);
        public IRepository<User> Users => _users ??= new UserRepository(_connectionString);
        public IRepository<Cart> Carts => _carts ??= new CartRepository(_connectionString);
        public ICartItemRepository CartItems => _cartItems ??= (ICartItemRepository) new CartItemRepository(_connectionString);
        public IRepository<Order> Orders => _orders ??= new OrderRepository(_connectionString);
        public IRepository<OrderItem> OrderItems => _orderItems ??= new OrderItemRepository(_connectionString);
        public IRepository<Payment> Payments => _payments ??= new PaymentRepository(_connectionString);
        public IRepository<Address> Addresses => _addresses ??= new AddressRepository(_connectionString);
        public IRepository<ShippingInfo> ShippingInfos => _shippingInfos ??= new ShippingInfoRepository(_connectionString);
        public IReviewRepository Reviews => _reviews ??= (IReviewRepository) new ReviewRepository(_connectionString);
        public IRepository<Role> Roles => _roles ??= new RoleRepository(_connectionString);
        public IUserRoleRepository UserRoles => _userRoles ??= (IUserRoleRepository) new UserRoleRepository(_connectionString);
        public IRepository<Permission> Permissions => _permissions ??= new PermissionRepository(_connectionString);
        public IRolePermissionRepository RolePermissions => _rolePermissions ??= (IRolePermissionRepository) new RolePermissionRepository(_connectionString);
        public IRepository<AdminLog> AdminLogs => _adminLogs ??= new AdminLogRepository(_connectionString);
        public IRepository<Warehouse> Warehouses => _warehouses ??= new WarehouseRepository(_connectionString);
        public IRepository<StockMovement> StockMovements => _stockMovements ??= new StockMovementRepository(_connectionString);
        public IRepository<Discount> Discounts => _discounts ??= new DiscountRepository(_connectionString);
        public IRepository<TaxRule> TaxRules => _taxRules ??= new TaxRuleRepository(_connectionString);
        public IRepository<ShippingMethod> ShippingMethods => _shippingMethods ??= new ShippingMethodRepository(_connectionString);
        public IWishlistRepository Wishlists => _wishlists ??= (IWishlistRepository) new WishlistRepository(_connectionString);
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