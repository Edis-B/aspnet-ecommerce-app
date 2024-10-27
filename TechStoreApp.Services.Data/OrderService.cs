using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data;
using TechStoreApp.Data.Models;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels;
using TechStoreApp.Web.ViewModels.Address;
using TechStoreApp.Web.ViewModels.Cart;
using TechStoreApp.Web.ViewModels.Orders;
using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Services.Data
{
    public class OrderService : IOrderService
    {
        private readonly TechStoreDbContext context;
        private readonly IUserService userService;
        public OrderService(TechStoreDbContext _context, IUserService _userService)
        {
            context = _context;
            userService = _userService;

        }
        public async Task<OrderViewModel> GetOrderViewModelAsync()
        {
            var userId = userService.GetUserId();

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Addresses)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            var newModel = new OrderViewModel();

            newModel.UserAddresses = context.Addresses
                .Where(a => a.UserId == userId)
                .Select(a => new AddressViewModel()
                {
                    Country = a.Country,
                    City = a.City,
                    PostalCode = a.PostalCode,
                    Details = a.Details,
                    Id = a.AddressId
                })
                .ToList();

            if (user.Cart == null || !user.Cart.CartItems.Any())
            {
                return newModel;
            }

            newModel.TotalCost = user.Cart.CartItems
                .Sum(ci => ci.Product.Price * ci.Quantity);

            newModel.CartItems = await context.CartItems
                .Where(ci => ci.CartId == user.Cart.CartId)
                .Select(ci => new CartItemViewModel()
                {
                    Quantity = ci.Quantity,
                    CartId = ci.CartId,
                    ProductId = ci.ProductId,
                    Product = new ProductViewModel()
                    {
                        ProductId = ci.Product.ProductId,
                        CategoryId = ci.Product.CategoryId,
                        Name = ci.Product.Name,
                        Price = ci.Product.Price,
                        Description = ci.Product.Description,
                        Stock = ci.Product.Stock,
                        ImageUrl = ci.Product.ImageUrl,
                    }
                })
                .ToListAsync();

            return newModel;
        }
        public async Task<OrderFinalizedModel> GetOrderFinalizedModelAsync(OrderViewModel model)
        {
            var userId = userService.GetUserId();

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                     .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();


            var newAddress = new AddressFormModel
            {
                Country = model.Address.Country,
                City = model.Address.City,
                PostalCode = model.Address.PostalCode,
                Details = model.Address.Details
            };

            model.Address = newAddress;

            var totalCost = user.Cart.CartItems
                .Sum(ci => ci.Product.Price * ci.Quantity);

            var newModel = new OrderFinalizedModel
            {
                Address = model.Address,
                Cart = await context.Carts
                    .Where(c => c.UserId == userId)
                    .Select(c => new CartViewModel()
                    {
                        CartItems = c.CartItems.Select(ci => new CartItemViewModel()
                        {
                            Quantity = ci.Quantity,
                            CartId = ci.CartId,
                            ProductId = ci.ProductId,
                            Product = new ProductViewModel()
                            {
                                ProductId = ci.Product.ProductId,
                                CategoryId = ci.Product.CategoryId,
                                Name = ci.Product.Name,
                                Price = ci.Product.Price,
                                Description = ci.Product.Description,
                                Stock = ci.Product.Stock,
                                ImageUrl = ci.Product.ImageUrl,
                            }
                        })
                        .ToList()
                    })
                    .FirstOrDefaultAsync() ?? new CartViewModel(),
                TotalSum = totalCost,
            };

            return newModel;
        }
        public async Task SendOrderAsync(SendOrderViewModel model)
        {
            var userId = userService.GetUserId();

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            var shippingAddressString = $"{model.Address.Country}, {model.Address.City} ({model.Address.PostalCode}), {model.Address.Details}";

            var totalCost = user.Cart.CartItems
                .Sum(ci => ci.Product.Price * ci.Quantity);

            var newOrder = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalCost,
                ShippingAddress = shippingAddressString,
                StatusId = 1
            };

            // Add order to database
            await context.AddAsync(newOrder);
            await context.SaveChangesAsync();

            var cartItems = user.Cart.CartItems;

            // Seed context with the order's details
            foreach (var item in cartItems)
            {
                var newOrderDetail = new OrderDetail()
                {
                    OrderId = newOrder.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                };

                var productsToDecrease = await context.Products
                    .FindAsync(item.ProductId);

                productsToDecrease.Stock = productsToDecrease.Stock -= item.Quantity;

                await context.AddAsync(newOrderDetail);
            }

            // Remove cart entries for user
            context.CartItems.RemoveRange(cartItems);
            context.Carts.Remove(user.Cart);

            await context.SaveChangesAsync();
        }
        public async Task<Address> GetAddressByIdAsync(int id)
        {
            var address = await context.Addresses
                .FindAsync(id);

            return address;
        }
    }
}
