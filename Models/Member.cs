using Microsoft.AspNetCore.Identity;

namespace BugTracker.Models
{
	public class Member : IdentityUser
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }

		public ICollection<ProjectMember>? MemberProjects { get; set; }
		public bool IsSelected { get; set; }
		public string FullName() { return FirstName + " " + LastName; }
	}
}
