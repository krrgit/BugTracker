using BugTracker.Models;

namespace BugTracker.Interfaces
{
	public interface IProjectRepository
	{

		public Task<IEnumerable<Project>> GetAll();
		public Task<Project> GetByIdAsync(int id);
		public Task<IEnumerable<Project>> GetUserProjects(int id);
		public Task<IEnumerable<AppUser>> GetMembers(int id);
		public Task<IEnumerable<Ticket>> GetTickets(int id);

		bool Add(Project project);
		bool Update(Project project);
		bool Delete(Project project);
		bool Save();

		bool AddTicket(Ticket ticket);

		bool AddAppUserToProject(AppUser appUser);
	}
}
