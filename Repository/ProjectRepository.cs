using BugTracker.Data;
using BugTracker.Interfaces;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repository
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly AppDBContext _context;

		public async Task<IEnumerable<Project>> GetAll()
		{
			return await _context.Projects.ToListAsync();
		}

		public ProjectRepository(AppDBContext context) {
			_context = context;
		}

		public async Task<Project> GetByIdAsync(int id)
		{
			return await _context.Projects.FindAsync(id);
		}

		public async Task<IEnumerable<AppUser>> GetMembers(int id)
		{
			var members = _context.Users
				.Include(m => m.MemberProjects)
					.ThenInclude(mp => mp.Project)
						.Where(m => m.MemberProjects.Any(mp => mp.ProjectId == id));

			var memberList = await members.ToListAsync();

			return memberList;
		}

		public async Task<IEnumerable<Ticket>> GetTickets(int id)
		{
			return await _context.Tickets.Where(t => t.ProjectId == id).ToListAsync();
		}

		public Task<IEnumerable<Project>> GetUserProjects(int id)
		{
			throw new NotImplementedException();
		}

		public bool Add(Project project)
		{
			throw new NotImplementedException();
		}

		public bool Delete(Project project)
		{
			throw new NotImplementedException();
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}

		public bool Update(Project project)
		{
			throw new NotImplementedException();
		}

		public bool AddTicket(Ticket ticket)
		{
			_context.Add(ticket);
			return _context.SaveChanges() > 0;
		}

		bool AddAppUserToProject(AppUser appUser, Project project) {
			var appUserProject = new AppUserProject() {
				AppUserId = appUser.Id,
				AppUser = appUser,
				ProjectId = project.Id,
				Project = project,
			};

			_context.Add(appUserProject);
			return _context.SaveChanges() > 0;
		}

		public bool AddAppUserToProject(AppUser appUser)
		{
			throw new NotImplementedException();
		}
	}
}
