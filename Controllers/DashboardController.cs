using BugTracker.Data;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(AppDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
