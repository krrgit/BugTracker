using Microsoft.AspNetCore.Identity;

namespace BugTracker.Models
{
	public class AppUser : IdentityUser
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public ICollection<AppUserProject>? MemberProjects { get; set; }
		public bool IsSelected { get; set; }
	}
}
