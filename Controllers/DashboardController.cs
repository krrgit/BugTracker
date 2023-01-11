using BugTracker.Data;
using BugTracker.Data.Enum;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(AppDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboardVM = new DashboardViewModel() {
                ProjectProgress = new List<int[]>(),
                ProjectTitles = new List<string>()
            };

            // For Every Project
            foreach (var project in _projecLinkVM)
            {
                var tickets = await _context.Tickets.Include(t => t.AssignedDev).Where(t => t.ProjectId == project.Id && t.AssignedDev.Id == userId).ToListAsync();

                var progress = new int[5];
                bool addToList = false;

                // For Every Ticket
                foreach (var ticket in tickets)
                {
                    ++progress[(int)ticket.Status];
					addToList = true;

				}

                if (addToList)
                {
                    dashboardVM.ProjectProgress.Add(progress);
                    dashboardVM.ProjectTitles.Add(project.Title);
                }
            }

            return View(dashboardVM);
        }
    }
}
