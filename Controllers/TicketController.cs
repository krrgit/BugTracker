using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
	public class TicketController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
