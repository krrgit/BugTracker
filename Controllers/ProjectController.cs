using BugTracker.Data;
using BugTracker.Migrations;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;


namespace BugTracker.Controllers
{
	public class ProjectController : Controller
	{
		private readonly AppDBContext _context;

		public ProjectController(AppDBContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			//var projects = _projectRepository.GetAll().Result.ToList();
			var projects = await _context.Projects.ToListAsync();
			return View(projects);
		}

		public async Task<IActionResult> Detail(int id)
		{
			Project project = await _context.Projects.FirstOrDefaultAsync(i => i.Id == id);

			Console.WriteLine("Project ID:" + id);

			var tickets = await _context.Tickets.Where(t => t.ProjectId == id).ToListAsync();
			var members = await _context.AppUsers
				.Include(m => m.MemberProjects)
					.ThenInclude(mp => mp.Project)
						.Where(m => m.MemberProjects.Any(mp => mp.ProjectId == id)).ToListAsync();


			var appUsers = await _context.AppUsers.Where(a => !members.Contains(a)).ToListAsync();

			ProjectViewModel projectVM = new ProjectViewModel()
			{
				Id = id,
				Title = project.Title,
				Description = project.Description,
				Tickets = tickets,
				Members = await TeamToViewModel(members),
				AppUsers = appUsers
			};

			return View(projectVM);
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
				catch(Exception ex)
				{
					role = "submitter";
                }
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
		public async Task<IActionResult> Detail(ProjectViewModel model)
		{

			if (ModelState.IsValid)
			{
				var project = await _context.Projects.FindAsync(model.Id);
				foreach (var item in model.AppUsers)
				{
					if (item.IsSelected)
					{
						Member user = await _context.AppUsers.FindAsync(item.Id);
						var appUserProject = new ProjectMember()
						{
							AppUserId = user.Id,
							AppUser = user,
							ProjectId = model.Id,
							Project = project
						};

						_context.Add(appUserProject);
						_context.SaveChanges();
					}
				}
				return RedirectToAction("Detail", new { id = model.Id });
			}

			ModelState.AddModelError("", "Error");
			return RedirectToAction("Index");
		}


	}
}
