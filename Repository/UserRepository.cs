using BugTracker.Data;
using BugTracker.Interfaces;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDBContext _context;

		public UserRepository(AppDBContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Member>> GetAllUsers()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<Member> GetUserById(string id)
		{
			return await _context.Users.FindAsync(id);
		}
		
		// Return members of a given project
		public async Task<IEnumerable<Member>> GetProjectMembers(int projectId)
		{
			var members = _context.Users
				.Include(m => m.MemberProjects)
					.ThenInclude(mp => mp.Project)
						.Where(m => m.MemberProjects.Any(mp => mp.ProjectId == projectId));

			var memberList = await members.ToListAsync();

			return memberList;
		}

		public async Task<IEnumerable<Project>> GetUserProjects(string memberId)
		{
			var projects = _context.Projects
				.Include(m => m.ProjectMembers)
					.ThenInclude(mp => mp.Project)
						.Where(m => m.ProjectMembers.Any(mp => mp.AppUserId == memberId));

			var list = await projects.ToListAsync();

            return list;
        }

        public string GetUserRole(string memberId)
		{
			return _context.Roles.FindAsync(memberId).Result.Name;

		}

        public bool Add(Member user)
		{
			throw new NotImplementedException();
		}

		public bool Delete(Member user)
		{
			throw new NotImplementedException();
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}

		public bool Update(Member user)
		{
			throw new NotImplementedException();
		}
	}
}
