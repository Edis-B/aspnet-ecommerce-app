using Azure.Core;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.Cart;

namespace TechStoreApp.Services.Data
{
    public class RequestService : IRequestService
    {

        public async Task<TType> GetProductIdFromRequest<TType>(HttpRequest request)
        {
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<TType>(body);

            return model;
        }

        public bool IsAjaxRequest(HttpRequest request)
        {
            return (request.Headers["Content-Type"] == "application/json");
        }
    }
}
