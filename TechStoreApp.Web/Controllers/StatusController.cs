using Microsoft.AspNetCore.Mvc;
using TechStoreApp.Services.Data.Interfaces;

namespace TechStoreApp.Web.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusService statusService;
        public StatusController(IStatusService _statusService) 
        {
            statusService= _statusService;
        }

    }
}
