using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Models;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface IUserService
    {
        string GetUserId();

        Task<ApplicationUser> GetUserByTheirId();
    }
}
