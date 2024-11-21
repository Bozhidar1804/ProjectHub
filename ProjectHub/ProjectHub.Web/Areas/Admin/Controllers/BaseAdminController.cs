using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Web.Areas.Admin.Controllers
{

    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class BaseAdminController : Controller
    {
    }
}
