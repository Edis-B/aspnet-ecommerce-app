using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;
using TechStoreApp.Data.Repository.Interfaces;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Services.Data
{
   
    public class StatusService : IStatusService
    {
        private readonly IRepository<Order, int> orderRepository;
        public StatusService(IRepository<Order, int> _orderRepository)
        {
            orderRepository = _orderRepository;
        }

        public async Task<bool> EditStatusOfOrder(int orderId, int statusId)
        {
           var result = await orderRepository.GetAllAttached()
                .Where(o => o.OrderId == orderId)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }

            result.StatusId = statusId;
            orderRepository.Update(result);

            return true;
        }
    }
}
