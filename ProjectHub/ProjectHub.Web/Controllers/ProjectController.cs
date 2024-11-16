using Microsoft.AspNetCore.Mvc;

namespace ProjectHub.Web.Controllers
{
    public class ProjectController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
