﻿using BugTracker.Data;
using BugTracker.Extensions;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics;


namespace BugTracker.Controllers
{
	public class ProjectController : BaseController
	{

		public ProjectController(AppDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
		{
		}

		public async Task<IActionResult> Index()
		{
			// Return all project for admin
			if (User.IsInRole("admin"))
			{
				var projects = await _context.Projects.ToListAsync();
				var projectsVM = new List<ProjectLinkViewModel>();

				foreach (var p in projects)
				{
					var project = new ProjectLinkViewModel()
					{
						Id = p.Id,
						Title = p.Title,
						Description = p.Description
					};

					projectsVM.Add(project);
				}

				return View(projectsVM);
			}

			// Return projects where user is apart of team
			return View(_projecLinkVM);
		}

		public async Task<IActionResult> Detail(int id)
		{
			Project project = await _context.Projects.FirstOrDefaultAsync(i => i.Id == id);

			Console.WriteLine("Project ID:" + id);

			var tickets = await _context.Tickets.Where(t => t.ProjectId == id).ToListAsync();
			tickets.OrderBy(t => t.UpdatedAt);
			tickets.Reverse();

			var members = await _context.AppUsers
				.Include(m => m.MemberProjects)
					.ThenInclude(mp => mp.Project)
						.Where(m => m.MemberProjects.Any(mp => mp.ProjectId == id)).ToListAsync();


			var appUsers = await _context.AppUsers.Where(a => !members.Contains(a)).ToListAsync();

            ProjectDetailViewModel projectVM = new ProjectDetailViewModel()
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
		public async Task<IActionResult> Detail(ProjectDetailViewModel model)
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

		public async Task<IActionResult> Create()
		{
			var appUsers = await _context.AppUsers.ToListAsync();
			var projectVM = new CreateProjectViewModel()
			{
				AppUsers = appUsers,
			};
			return View(projectVM);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateProjectViewModel projectVM)
		{
			if (ModelState.IsValid)
			{
				var project = new Project()
				{
					Title = projectVM.Title,
					Description = projectVM.Description
				};

                Member member = await _context.AppUsers.FindAsync(userId);

				if (member == null)
				{
                    ModelState.AddModelError("", "Error getting user");
                    return View(projectVM);
				}


                _context.Add(project);
				_context.SaveChanges();


                var projectMember = new ProjectMember()
                {
                    AppUserId = member.Id,
                    AppUser = member,
                    ProjectId = project.Id,
                    Project = project
                };

                _context.Add(projectMember);
                _context.SaveChanges();
				return RedirectToAction("Index", "Project");
			}
			return View(projectVM);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var project = await _context.Projects.FindAsync(id);
			if (project == null) return View("Error");

			var projectVM = new EditProjectViewModel()
			{
				Id = project.Id,
				Title = project.Title,
				Description = project.Description
			};
			return View(projectVM);
		}


		[HttpPost]
		public async Task<IActionResult> Edit(EditProjectViewModel projectVM)
		{
			if (!ModelState.IsValid)
			{
				return View(projectVM);
			}

			var project = await _context.Projects.FindAsync(projectVM.Id);

			if (project == null)
			{
				return View(projectVM);
			}

			project.Title = projectVM.Title;
			project.Description = projectVM.Description;

			_context.Update(project);
			_context.SaveChanges();
			return RedirectToAction("Detail", new { id = projectVM.Id });
		}
	}

}
