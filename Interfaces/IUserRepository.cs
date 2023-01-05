using BugTracker.Models;

namespace BugTracker.Interfaces
{
	public interface IUserRepository
	{
		Task<IEnumerable<Member>> GetAllUsers();
		Task<IEnumerable<Member>> GetProjectMembers(int projectId);
		Task<Member> GetUserById(string id);
		Task<IEnumerable<Project>> GetUserProjects(string memberId);

		string GetUserRole(string memberId);
        bool Add(Member user);
		bool Update(Member user);
		bool Delete(Member user);
		bool Save();
	}
}
