using BugTracker.Data;
using BugTracker.Migrations;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
	public class ProjectMemberController : BaseController
	{

		public ProjectMemberController(AppDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
		}

		[HttpPost]
		public async Task<IActionResult> Add(ProjectDetailViewModel model)
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
				return RedirectToAction("Detail", "Project", new { id = model.Id });
			}

			ModelState.AddModelError("", "Error");
			return RedirectToAction("Detail", "Project", new { id = model.Id });
		}

		[HttpPost]
		public async Task<IActionResult> Remove(int projectId, string appUserId)
		{
			if (ModelState.IsValid)
			{
				var memberProject = await _context.ProjectMembers.FindAsync(appUserId, projectId);

				_context.Remove(memberProject);
				_context.SaveChanges();
			} else
			{
				ModelState.AddModelError("", "Error");
			}

			return RedirectToAction("Detail", "Project", new { id = projectId });
		}
	}
}
