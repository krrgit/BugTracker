using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Views.Project
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
