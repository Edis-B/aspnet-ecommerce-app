using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechStoreApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/")]
    [Authorize]
    public class ProductsApiController : ControllerBase
    {

    }
}
