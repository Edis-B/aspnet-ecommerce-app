using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface ISeedDataService
    {
        Task SeedAllMissingData();
        Task SeedRoles();
        Task SeedAccounts();
        Task SeedCategories();
        Task SeedProducts();
        Task SeedStatuses();
    } 
}
