using BugTracker.Data;
using BugTracker.Data.Enum;
using BugTracker.Models;
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
            DashboardViewModel dashboardVM = new DashboardViewModel();

            if (User.IsInRole("submitter"))
            {
                dashboardVM = await GetSubmitterDashboardVM();
            } else
            {
                dashboardVM = await GetOtherDashboardVM();
            }

            return View(dashboardVM);
        }

        async Task<DashboardViewModel> GetSubmitterDashboardVM()
        {
            DashboardViewModel dashboardVM = new DashboardViewModel()
            {
                ProjectProgress = new List<int[]>(),
                ProjectTitles = new List<string>()
            };

            dashboardVM.TicketLists = new List<List<Ticket>>();

            // For Every Project
            foreach (var project in _projecLinkVM)
            {
                var tickets = await _context.Tickets.Include(t => t.Author).Include(t => t.AssignedDev).Where(t => t.ProjectId == project.Id && t.Author.Id == userId).ToListAsync();

                // Sort by Last Updated Date; latest = first
                tickets.OrderBy(t => t.UpdatedAt);
                tickets.Reverse();

                // Used to display the progress of a project
                var progress = new int[5];

                // Get a list of tickets of this project assigned to this user
                var userTickets = new List<Ticket>();

                // For Every Ticket assigned by submitter
                foreach (var ticket in tickets)
                {
                    if (ticket.Author.Id == userId)
                    {
                        // Only add to the list if its not resolved
                        userTickets.Add(ticket);

                        ++progress[(int)ticket.Status];
                    }
                }

                dashboardVM.ProjectTitles.Add(project.Title);
                dashboardVM.TicketLists.Add(userTickets);
                dashboardVM.ProjectProgress.Add(progress);
            }
            return dashboardVM;
        }

        async Task<DashboardViewModel> GetOtherDashboardVM()
        {
            DashboardViewModel dashboardVM = new DashboardViewModel()
            {
                ProjectProgress = new List<int[]>(),
                ProjectTitles = new List<string>()
            };

            dashboardVM.TicketLists = new List<List<Ticket>>();

            // For Every Project
            foreach (var project in _projecLinkVM)
            {

                var tickets = await _context.Tickets.Include(t => t.Author).Include(t => t.AssignedDev).Where(t => t.ProjectId == project.Id && t.AssignedDev.Id == userId).ToListAsync();

                // Sort by Last Updated Date; latest = first
                tickets.OrderBy(t => t.UpdatedAt);
                tickets.Reverse();

                // Used to display the progress of a project
                var progress = new int[5];

                // Get a list of tickets of this project assigned to this user
                var userTickets = new List<Ticket>();

                // For Every Ticket
                foreach (var ticket in tickets)
                {
                    if (ticket.AssignedDev.Id == userId)
                    {
                        // Only add to the list if its not resolved
                        if (ticket.Status != TicketStatus.Resolved)
                        {
                            userTickets.Add(ticket);
                        }

                        ++progress[(int)ticket.Status];
                    }
                }

                dashboardVM.ProjectTitles.Add(project.Title);
                dashboardVM.TicketLists.Add(userTickets);
                dashboardVM.ProjectProgress.Add(progress);
            }
            return dashboardVM;
        }
    }
}
