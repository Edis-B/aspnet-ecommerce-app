using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using TechStoreApp.Web.ViewModels;

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

        public static void AppendErrorViewModelToTempData(ITempDataDictionary tempdata, string key, IEnumerable<string> result, int statusCode)
        {
            var errorModel = new ErrorViewModel()
            {
                Messages = result.ToList(),
                StatusCode = 403
            };

            tempdata[key] = JsonConvert.SerializeObject(errorModel);
        }
    }
}
