using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TechStoreApp.Services.Data.Interfaces;
using static TechStoreApp.Common.GeneralConstraints;

namespace TechStoreApp.Services.Data
{
    public class CookieService : ICookieService
    {
        private readonly IUserService userService;
        private readonly IDataProtector protector;

        public CookieService(IUserService _userService,
            IDataProtectionProvider dataProtectionProvider) 
        {
            protector = dataProtectionProvider.CreateProtector(SecretCookieProtectionKey);
            userService = _userService;
        }
        public void AppendToCookie(string newKey, string newValue, HttpContext context)
        {
            var protectedCookie = context.Request.Cookies[IsAdminAttr];

            // Unprotecting Cookie
            string unprotectedCookie;
            if (protectedCookie != null)
            {
                unprotectedCookie = protector.Unprotect(protectedCookie!);
            }
            else unprotectedCookie = protectedCookie!;
            
            // Fill new cookie with old data
            var cookieData = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(unprotectedCookie))
            {
                cookieData = JsonSerializer.Deserialize<Dictionary<string, string>>(unprotectedCookie);
            }

            // Attach new data also
            cookieData![newKey] = newValue;

            // Reprotect cookie
            var updatedCookieValue = JsonSerializer.Serialize(cookieData);
            var protectedValue = protector.Protect(updatedCookieValue);

            context.Response.Cookies.Append(IsAdminAttr, protectedValue, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            });
        }

        public async Task AttachIsUserAdminToCookie(HttpContext context)
        {
            var isAdmin = await userService.IsUserAdmin(userService.GetUserId());

            string newKey = IsAdminAttr;
            string newValue = isAdmin.ToString();
            this.AppendToCookie(newKey, newValue, context);
        }
    }
}

