using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Web.ViewModels.Home;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetHomeViewModel();
    }
}
