using Microsoft.AspNetCore.Mvc;

namespace ProjectHub.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
