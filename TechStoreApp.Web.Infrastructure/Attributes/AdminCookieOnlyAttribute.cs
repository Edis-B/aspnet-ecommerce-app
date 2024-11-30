using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using static TechStoreApp.Common.GeneralConstraints;

namespace CinemaApp.Web.Infrastructure.Attributes 
{
    public class AdminCookieOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null)
            {
                // Protector
                var dataProtectionProvider = context.HttpContext.RequestServices.GetService<IDataProtectionProvider>();
                var protector = dataProtectionProvider!.CreateProtector(SecretCookieProtectionKey);

                // Get Cookie
                string protectedValue;
                bool retrieveCookieResult =
                    context.HttpContext.Request.Cookies.TryGetValue(IsAdminAttr, out protectedValue!);

                if (!retrieveCookieResult)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Unprotect Cookie
                var unprotectedValue = protector.Unprotect(protectedValue);

                var cookieData = JsonSerializer.Deserialize<Dictionary<string, string>>(unprotectedValue);

                // Parse content
                bool isAdmin = false;
                if (cookieData != null && cookieData.ContainsKey(IsAdminAttr))
                {
                    isAdmin = (bool.TryParse(cookieData[IsAdminAttr], out bool parsedIsAdmin)) 
                        ? parsedIsAdmin 
                        : false;
                }

                // Check result
                if (!isAdmin)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
