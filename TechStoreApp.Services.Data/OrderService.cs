using Microsoft.EntityFrameworkCore;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.ApiViewModels.Orders;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Orders;
using TechStoreApp.Web.ViewModels.Products;
using static TechStoreApp.Common.GeneralConstraints;

namespace TechStoreApp.Services.Data
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order, int> orderRepository;
        private readonly IRepository<ApplicationUser, Guid> userRepository;
        private readonly IRepository<Product, int> productRepository;
        private readonly IRepository<OrderDetail, int> orderDetailRepository;
        private readonly IRepository<Cart, int> cartRepository;
        private readonly IRepository<CartItem, object> cartItemRepository;
        private readonly IUserService userService;
        private readonly IRepository<Status, int> statusRepository;
        public OrderService(IRepository<Order, int> _orderRepository,
            IRepository<ApplicationUser, Guid> _userRepository,
            IRepository<Product, int> _productRepository,
            IRepository<OrderDetail, int> _orderDetailRepository,
            IRepository<Cart, int> _cartRepository,
            IRepository<CartItem, object> _cartItemRepository,
            IUserService _userService,
            IRepository<Status, int> _statusRepository)
        {
            userRepository = _userRepository;
            orderRepository = _orderRepository;
            productRepository = _productRepository;
            orderDetailRepository = _orderDetailRepository;
            cartRepository = _cartRepository;
            cartItemRepository = _cartItemRepository;
            userService = _userService;
            statusRepository = _statusRepository;
        }
        public async Task<OrderPageViewModel> GetOrderViewModelAsync(int? addressId)
        {
            var userId = userService.GetUserId();

            var user = await userRepository.GetAllAttached()
                .Where(u => u.Id == userId)
                .Include(u => u.Addresses)
                .Include(u => u.Cart)
                    .ThenInclude(c => c!.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            var newModel = new OrderPageViewModel();

            if ((user!).Cart == null || !(user.Cart.CartItems.Any()))
            {
                return newModel;
            }

            // Also check if user is owner of address
            if (addressId != null && !user.Addresses.Any(a => a.AddressId == addressId))
            {
                newModel.Address = user.Addresses
                    .Select(a => new AddressFormModel()
                    {
                        Country = a.Country,
                        City = a.City,
                        PostalCode = a.PostalCode.ToString(),
                        Details = a.Details,
                        Id = a.AddressId,
                    })
                    .FirstOrDefault()!;
            }

            newModel.AllUserAddresses = user!.Addresses
                .Select(a => new AddressViewModel()
                {
                    //Country = a.Country,
                    //City = a.City,
                    //PostalCode = a.PostalCode,
                    Details = a.Details,
                    Id = a.AddressId,
                })
                .ToList();


            newModel.TotalCost = user.Cart.CartItems
                .Sum(ci => ci.Product!.Price * ci.Quantity);

            newModel.CartItems = user.Cart.CartItems
                .Select(ci => new CartItemViewModel()
                {
                    Quantity = ci.Quantity,
                    CartId = ci.CartId,
                    ProductId = ci.ProductId,
                    Product = new ProductViewModel()
                    {
                        ProductId = ci.Product!.ProductId,
                        CategoryId = ci.Product.CategoryId,
                        Name = ci.Product.Name,
                        Price = ci.Product.Price,
                        Description = ci.Product.Description,
                        Stock = ci.Product.Stock,
                        ImageUrl = ci.Product.ImageUrl,
                    }
                })
                .ToList();

            return newModel;
        }

        public async Task<OrderFinalizedPageViewModel> GetOrderFinalizedModelAsync(OrderPageViewModel model)
        {
            var userId = userService.GetUserId();

            if (userId == default)
            {
                return default!;
            }

            var user = await userRepository.GetAllAttached()
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                     .ThenInclude(c => c!.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            if (user == null || user.Cart == null || user.Cart.CartItems == null)
            {
                return default!;
            }

            var totalCost = user.Cart.CartItems
                .Sum(ci => ci.Product!.Price * ci.Quantity);

            var newModel = new OrderFinalizedPageViewModel
            {
                Address = model.Address,
                PaymentMethod = model.PaymentId,
                TotalSum = totalCost,
                Cart = new CartViewModel()
                    {
                        CartItems = user.Cart.CartItems.Select(ci => new CartItemViewModel()
                        {
                            Quantity = ci.Quantity,
                            CartId = ci.CartId,
                            ProductId = ci.ProductId,
                            Product = new ProductViewModel()
                            {
                                ProductId = ci.Product!.ProductId,
                                CategoryId = ci.Product.CategoryId,
                                Name = ci.Product.Name,
                                Price = ci.Product.Price,
                                Description = ci.Product.Description,
                                Stock = ci.Product.Stock,
                                ImageUrl = ci.Product.ImageUrl,
                            }
                        })
                        .ToList()
                    } ?? new CartViewModel(),
            };

            return newModel;
        }

        public async Task SendOrderAsync(SendOrderViewModel model)
        {
            var userId = userService.GetUserId();

            var user = await userRepository.GetAllAttached()
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c!.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            if (user == null || user.Cart == null || user.Cart.CartItems == null || user.Cart.CartItems.Count == 0)
            {
                return;
            }

            var shippingAddressString = $"{model.Address.Country}, {model.Address.City} ({model.Address.PostalCode}), {model.Address.Details}";

            var totalCost = user.Cart.CartItems
                .Sum(ci => ci.Product!.Price * ci.Quantity);

            var newOrder = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalCost,
                ShippingAddress = shippingAddressString,
                StatusId = 1
            };

            // Add order to database
            await orderRepository.AddAsync(newOrder);

            var cartItems = user.Cart.CartItems;

            // Seed context with the order's details
            foreach (var item in cartItems)
            {
                var newOrderDetail = new OrderDetail()
                {
                    OrderId = newOrder.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product!.Price
                };

                var productsToDecrease = await productRepository
                    .GetByIdAsync(item.ProductId);

                // Lower stock
                productsToDecrease.Stock = productsToDecrease.Stock -= item.Quantity;
                await productRepository.UpdateAsync(productsToDecrease);

                // Reduce stock to limit if anyone's cart is over the limit
                var othersItems = cartItemRepository.GetAllAttached()
                    .Where(ci => ci.ProductId == item.ProductId)
                    .Where(ci => ci.Quantity >= productsToDecrease.Stock);

                foreach (var otherItem in othersItems)
                {
                    otherItem.Quantity = productsToDecrease.Stock;
                    await cartItemRepository.UpdateAsync(otherItem);
                }

                await orderDetailRepository.AddAsync(newOrderDetail);
            }

            // Remove cart entries for user
            await cartItemRepository.RemoveRangeAsync(cartItems);
            await cartRepository.DeleteAsync(user.Cart.CartId);
        }

        public async Task<UserOrdersListViewModel> GetUserOrdersListViewModelAsync(string? userId = null)
        {
            var orders = orderRepository.GetAllAttached();

            userId = userId 
                ?? userService.GetUserId().ToString();

            orders = orders
                .Where(o => o.UserId.ToString() == userId);

            var results = await orders
                .Select(o => new UserOrderSingleViewModel()
                {
                    OrderId = o.OrderId,
                    ShippingAddress = o.ShippingAddress!,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                    OrderDetails = o.OrderDetails
                        .Select(od => new OrderDetailViewModel()
                        {
                            ProductImageUrl = od.Product!.ImageUrl ?? string.Empty,
                            ProductName = od.Product.Name,
                            ProductId = od.ProductId,
                            Quantity = od.Quantity,
                            UnitPrice = od.UnitPrice
                        })
                        .ToList()
                })
                .ToListAsync();

            var orderModel = new UserOrdersListViewModel()
            {
                Orders = results
            };

            return orderModel;
        }

        public async Task<UserOrderSingleViewModel> GetDetailsOfOrder(int orderId)
        {
            var order = await orderRepository.GetAllAttached()
                .Where(o => o.OrderId == orderId)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync();

            var userId = userService.GetUserId();

            if (order == null)
            {
                return null!;
            }

            if (order.UserId != userId && !await userService.IsUserAdmin(userId))
            {
                return null!;
            }

            var orderViewModel = new UserOrderSingleViewModel
            {
                OrderId = order!.OrderId,
                ShippingAddress = order.ShippingAddress!,
                OrderDate = order.OrderDate.ToString("dd/MM/yyyy"),
                TotalPrice = (double)order.TotalAmount,
                OrderDetails = order.OrderDetails
                    .Select(od => new OrderDetailViewModel
                    {
                        ProductImageUrl = od.Product?.ImageUrl ?? string.Empty,
                        ProductName = od.Product?.Name ?? string.Empty,
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    })
                    .ToList()
            };


            orderViewModel!.CurrentStatus = new KeyValuePair<string, int>(order!.Status.Description, order.Status.StatusId);

            var allStatuses = await statusRepository
                .GetAllAttached()
                .ToListAsync();

            orderViewModel!.Statuses = allStatuses
                .Where(s => s.StatusId != orderViewModel.CurrentStatus.Value)
                .Select(s => new KeyValuePair<string, int>(s.Description, s.StatusId))
                .ToList();

            return orderViewModel!;
        }

        public async Task<IEnumerable<OrderApiViewModel>> ApiGetAllOrders()
        {
            var orders = await orderRepository.GetAllAttached()
                .Include(o => o.User)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();

            var result = orders.Select(o => new OrderApiViewModel(o));

            return result;
        }

        public async Task<IEnumerable<OrderApiViewModel>> ApiGetAllOrdersFromUserId(string userId)
        {
            var orders = await orderRepository.GetAllAttached()
                .Where(o => o.UserId == Guid.Parse(userId))
                .Include(o => o.User)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();

            var result = orders.Select(o => new OrderApiViewModel(o));

            return result;
        }
    }
}
