using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IStatusService
    {
        Task<bool> EditStatusOfOrder(int orderId, int statusId);
    }
}
