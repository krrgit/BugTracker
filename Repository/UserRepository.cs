using BugTracker.Data;
using BugTracker.Interfaces;
using BugTracker.Models;
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
		public async Task<IEnumerable<AppUser>> GetAllUsers()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<AppUser> GetUserById(string id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<IEnumerable<AppUser>> GetProjectMembers(int projectId)
		{
			var members = _context.Users
				.Include(m => m.MemberProjects)
					.ThenInclude(mp => mp.Project)
						.Where(m => m.MemberProjects.Any(mp => mp.ProjectId == projectId));

			var memberList = await members.ToListAsync();

			return memberList;
		}

		public bool Add(AppUser user)
		{
			throw new NotImplementedException();
		}

		public bool Delete(AppUser user)
		{
			throw new NotImplementedException();
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}

		public bool Update(AppUser user)
		{
			throw new NotImplementedException();
		}
	}
}
