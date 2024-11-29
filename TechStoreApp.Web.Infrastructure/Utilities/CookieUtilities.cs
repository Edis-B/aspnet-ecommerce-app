using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using static TechStoreApp.Common.GeneralConstraints;

namespace TechStoreApp.Web.Infrastructure.Utilities
{
    public class CookieUtilities
    {
        public void AppendToCookie(string newKey, string newValue, HttpContext context)
        {
            // Cookie name
            var cookieName = MyCookieName;

            // Retrieve the existing cookie value
            var existingCookie = context.Request.Cookies[cookieName];

            // Parse the existing cookie value as a dictionary (assuming JSON format)
            var cookieData = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(existingCookie))
            {
                cookieData = JsonSerializer.Deserialize<Dictionary<string, string>>(existingCookie);
            }

            // Add or update the new key-value pair
            cookieData[newKey] = newValue;

            // Serialize the updated dictionary back to JSON
            var updatedCookieValue = JsonSerializer.Serialize(cookieData);

            // Set the updated cookie
            context.Response.Cookies.Append(cookieName, updatedCookieValue, new CookieOptions
            {
                HttpOnly = true,       // Secure the cookie
                IsEssential = true,    // Ensure it's not affected by GDPR settings
                Expires = DateTime.UtcNow.AddDays(7) // Optional: Set expiration date
            });
        }
    }
}
