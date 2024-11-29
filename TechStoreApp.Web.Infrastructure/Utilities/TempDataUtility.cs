using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TechStoreApp.Web.Infrastructure
{
    public static class TempDataUtility
    {
        public static T? GetTempData<T>(ITempDataDictionary tempdata, string key)
        {
            if (tempdata.Any(td => td.Key == key))
            {
                var objectAsJson = tempdata[key] as string;
                var result = JsonConvert.DeserializeObject<T>(objectAsJson!);

                return result;
            }

            return default;
        }
    }
}
