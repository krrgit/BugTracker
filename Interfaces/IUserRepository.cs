using BugTracker.Models;

namespace BugTracker.Interfaces
{
	public interface IUserRepository
	{
		Task<IEnumerable<AppUser>> GetAllUsers();
		Task<IEnumerable<AppUser>> GetProjectMembers(int projectId);
		Task<AppUser> GetUserById(string id);
		
		bool Add(AppUser user);
		bool Update(AppUser user);
		bool Delete(AppUser user);
		bool Save();
	}
}
