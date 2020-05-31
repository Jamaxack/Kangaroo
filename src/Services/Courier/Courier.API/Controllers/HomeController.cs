using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Courier.API.Controllers
{
    public class HomeController : Controller
    {
        readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var pathBase = _configuration["PATH_BASE"];
            return new RedirectResult($"~{pathBase}/swagger");
        }
    }
}
