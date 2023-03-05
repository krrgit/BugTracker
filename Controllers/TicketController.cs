using BugTracker.Data;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
	public class TicketController : BaseController
	{
		//private readonly AppDBContext _context;

		public TicketController(AppDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Create(int id)
		{
			var project = await _context.Projects.FindAsync(id);

			string userId = _httpContextAccessor.HttpContext?.User.GetUserId();

			var members = await _context.AppUsers
                .Include(m => m.MemberProjects)
                    .ThenInclude(mp => mp.Project)
                        .Where(m => m.MemberProjects.Any(mp => mp.ProjectId == project.Id)).ToListAsync();
            var ticketVM = new CreateTicketViewModel()
			{
				ProjectId = id,
				ProjectTitle = project.Title,
				TeamSelectList = members,
				AuthorId = userId
			};

			return View(ticketVM);
		}

        private async Task<List<TeamMemberViewModel>> TeamToViewModel(List<Member> members)
        {
            var teamMemberVMs = new List<TeamMemberViewModel>();

            foreach (var m in members)
            {
                string role = "submitter";
                try
                {
                    var roleId = (await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == m.Id)).RoleId;
                    role = (await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId)).Name;
                }
                catch (Exception ex)
                {
                    role = "submitter";
                }
				string FullName = m.FirstName + " " + m.LastName;
                var memberVM = new TeamMemberViewModel()
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Role = role
                };
                teamMemberVMs.Add(memberVM);
            }
            return teamMemberVMs;
        }



        [HttpPost]
		public async Task<IActionResult> Create(CreateTicketViewModel ticketVM)
		{
			if (ModelState.IsValid)
			{
				var author = await _context.Users.FindAsync(ticketVM.AuthorId);
				var assignDev = await _context.Users.FindAsync(ticketVM.AssignedDevId);
				var ticket = new Ticket
				{
					Title = ticketVM.Title,
					Description = ticketVM.Description,
					Priority = ticketVM.Priority,
					Status = Data.Enum.TicketStatus.New,
					Type = ticketVM.Type,
					ProjectId = ticketVM.Id,
					CreatedAt = DateTime.Now.ToUniversalTime(),
					UpdatedAt = DateTime.Now.ToUniversalTime(),
					Author = author,
					AssignedDev = assignDev
                };
				_context.Add(ticket);
				_context.SaveChanges();
				return RedirectToAction("Detail", "Project", new { id = ticketVM.Id });
			}

            var members = await _context.AppUsers
                .Include(m => m.MemberProjects)
                    .ThenInclude(mp => mp.Project)
                        .Where(m => m.MemberProjects.Any(mp => mp.ProjectId == ticketVM.ProjectId)).ToListAsync();

			ticketVM.TeamSelectList = members;

            ModelState.AddModelError("", "Error");
			return View(ticketVM);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{

			var ticket = await _context.Tickets.FindAsync(id);
			var project = await _context.Projects.FindAsync(ticket.ProjectId);
            var members = await _context.AppUsers
                .Include(m => m.MemberProjects)
                    .ThenInclude(mp => mp.Project)
                        .Where(m => m.MemberProjects.Any(mp => mp.ProjectId == project.Id)).ToListAsync();


			var ticketVM = new EditTicketViewModel()
			{
				Id = id,
				Title = ticket.Title,
				Description = ticket.Description,
                AssignedDevId = ticket.AssignedDev?.Id,
                AssignedDev = ticket.AssignedDev,
                AuthorId = ticket.Author?.Id,
				Priority = ticket.Priority,
				Status = ticket.Status,
				Type = ticket.Type,
				ProjectId = ticket.ProjectId,
				ProjectTitle = project.Title,
				CreatedAt = ticket.CreatedAt,
				UpdatedAt = ticket.UpdatedAt,
				TeamSelectList = members
			};

			return View(ticketVM);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditTicketViewModel ticketVM)
		{
			if (ModelState.IsValid)
			{
                ModelState.Remove("JobIndicator");
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
					Status = ticketVM.Status,
					Type = ticketVM.Type,
					ProjectId = ticketVM.ProjectId,
					CreatedAt = userTicket.CreatedAt,
					UpdatedAt = DateTime.Now.ToUniversalTime(),
				};

				if (ticketVM.AssignedDevId != null)
				{
					ticket.AssignedDev = await _context.Users.FindAsync(ticketVM.AssignedDevId);
				}

				if (ticketVM.AuthorId != null)
				{
                    ticket.Author = await _context.Users.FindAsync(ticketVM.AuthorId);
                }

                _context.Update(ticket);
				_context.SaveChanges();
				return RedirectToAction("Detail", "Ticket", new { id = ticketVM.Id });
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
			var ticket = await _context.Tickets.Include(t =>t.AssignedDev).Include(t => t.Author).FirstOrDefaultAsync(t => t.Id == id);

			return View(ticket);
		}
	}
}
