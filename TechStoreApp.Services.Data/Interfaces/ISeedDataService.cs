using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface ISeedDataService
    {
        Task SeedAllData();
        Task SeedRoles();
        Task SeedProducts();
        Task SeedCategories();
        Task SeedStatuses();
        Task SeedAccounts();

    }
}
