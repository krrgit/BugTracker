using BugTracker.Data;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    public class ProjectController : Controller
    {
        AppDBContext _context;

        public ProjectController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Project> projects = await _context.Projects.ToListAsync();
            return View(projects);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Project project = await _context.Projects.FindAsync(id);
            var tickets = await _context.Tickets.Where(t => t.ProjectId == id).ToListAsync();

            ProjectViewModel projectVM = new ProjectViewModel()
            {
                Id = id,
                Title = project.Title,
                Description = project.Description,
                Tickets = tickets
            };

            return View(projectVM);
        }

        public async Task<IActionResult> CreateTicket(int id)
		{
            var project = await _context.Projects.FindAsync(id);
			var ticketVM = new CreateTicketViewModel()
            {
                ProjectId = id,
				ProjectTitle = project.Title
			};

            return View(ticketVM);
        }

        [HttpPost]
        public IActionResult CreateTicket(CreateTicketViewModel ticketVM)
        {
            if (ModelState.IsValid)
            {
                var ticket = new Ticket
                {
                    Title = ticketVM.Title,
                    Description = ticketVM.Description,
                    Priority = ticketVM.Priority,
                    Status = Data.Enum.TicketStatus.New,
                    Type = ticketVM.Type,
                    ProjectId = ticketVM.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Add(ticket);
                _context.SaveChanges();
                return RedirectToAction("Detail", new { id = ticketVM.Id });
            }

            ModelState.AddModelError("", "Error");
            return View(ticketVM);
        }
    }
}
