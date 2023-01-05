using BugTracker.Data;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
	public class TicketController : Controller
	{
		private readonly AppDBContext _context;

		public TicketController(AppDBContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Create(int id)
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
		public IActionResult Create(CreateTicketViewModel ticketVM)
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
				return RedirectToAction("Detail", "Project", new { id = ticketVM.Id });
			}

			ModelState.AddModelError("", "Error");
			return View(ticketVM);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var ticket = await _context.Tickets.FindAsync(id);
			var project = await _context.Projects.FindAsync(ticket.ProjectId);
			var ticketVM = new EditTicketViewModel()
			{
				Id = id,
				Title = ticket.Title,
				Description = ticket.Description,
				AssignedDevId = ticket.AssignedDev?.Id,
				AuthorId = ticket.Author?.Id,
				Priority = ticket.Priority,
				Status = ticket.Status,
				Type = ticket.Type,
				ProjectId = ticket.ProjectId,
				ProjectTitle = project.Title,
				CreatedAt = ticket.CreatedAt,
				UpdatedAt = ticket.UpdatedAt
			};

			return View(ticketVM);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditTicketViewModel ticketVM)
		{
			if (ModelState.IsValid)
			{
				var userTicket = await _context.Tickets.AsNoTracking().FirstOrDefaultAsync(i => i.Id == ticketVM.Id);

				if (userTicket == null)
				{
					return RedirectToAction("Detail", "Project", new { id = ticketVM.Id });
				}

				var ticket = new Ticket
				{
					Id = ticketVM.Id,
					Title = ticketVM.Title,
					Description = ticketVM.Description,
					Priority = ticketVM.Priority,
					Status = Data.Enum.TicketStatus.New,
					Type = ticketVM.Type,
					ProjectId = ticketVM.ProjectId,
					CreatedAt = userTicket.CreatedAt,
					UpdatedAt = DateTime.Now
				};
				_context.Update(ticket);
				_context.SaveChanges();
				return RedirectToAction("Detail", "Project", new { id = ticketVM.ProjectId });
			}

			ModelState.AddModelError("", "Error");
			return View(ticketVM);
		}

		public async Task<IActionResult> Delete(int[] id)
		{
			var ticket = await _context.Tickets.FindAsync(id[0]);
			_context.Remove(ticket);
			_context.SaveChanges();

			return RedirectToAction("Detail", "Project", new { id = id[1] });
		}

		[HttpGet]
		public async Task<IActionResult> Detail(int id)
		{
			var ticket = await _context.Tickets.FindAsync(id);

			return View(ticket);
		}
	}
}
