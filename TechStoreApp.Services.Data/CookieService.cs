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
        private readonly HttpContext context;

        public CookieService(IUserService _userService,
            IDataProtectionProvider dataProtectionProvider,
            IHttpContextAccessor httpContextAccessor) 
        {
            protector = dataProtectionProvider.CreateProtector(SecretCookieProtectionKey);
            userService = _userService;
            context = httpContextAccessor.HttpContext!;
        }
        public void AppendToCookie(string newKey, string newValue, TimeSpan? duration = null)
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


            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,

                // Enable crosssite requests for Api
                SameSite = SameSiteMode.None,
                Domain = DomainStr,
                Path = "/"
            };

            if (duration.HasValue)
            {
                cookieOptions.Expires = DateTime.UtcNow.Add(duration.Value);
            }

            context.Response.Cookies.Append(IsAdminAttr, protectedValue, cookieOptions);
        }

        public async Task AttachIsUserAdminToCookie(bool rememberMe)
        {
            var userId = userService.GetUserId();
            var isAdmin = await userService.IsUserAdmin(userId);

            string newKey = IsAdminAttr;
            string newValue = isAdmin.ToString();

            if (rememberMe)
            {
                this.AppendToCookie(newKey, newValue, TimeSpan.FromDays(CookieDurationRememberMe));
            }
            else
            {
                this.AppendToCookie(newKey, newValue);
            }
        }
    }
}

