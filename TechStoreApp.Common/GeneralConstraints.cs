using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Common
{
    public static class GeneralConstraints
    {
        public const string AdminRoleName = "Admin";
        public const string MyCookieName = ".MyCustomCookie";
        public const string IsAdminAttr = "IsAdmin";
        public const string SecretCookieProtectionKey = "MySecureCookieProtectionKey";
        public const string DomainStr = ".localhost";
        public const int CookieDurationRememberMe = 7;
        public const int SlidingExpirationInMinutes = 30;
    }
}
