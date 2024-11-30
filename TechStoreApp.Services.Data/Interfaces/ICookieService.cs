using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Services.Data.Interfaces
{
    public interface ICookieService
    {
        public void AppendToCookie(string newKey, string newValue, TimeSpan? duration = null);
        public Task AttachIsUserAdminToCookie(bool rememberMe);
    }
}
