using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
