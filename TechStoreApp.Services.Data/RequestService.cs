using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Services.Data
{
    public class RequestService : IRequestService
    {

        public async Task<TType> ExtractModelFromRequestBody<TType>(HttpRequest request)
        {
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<TType>(body);

            return model ?? default!;
        }

        public bool IsAjaxRequest(HttpRequest request)
        {
            return (request.Headers["Content-Type"] == "application/json");
        }
    }
}
