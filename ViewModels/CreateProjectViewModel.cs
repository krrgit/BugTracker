using BugTracker.Models;

namespace BugTracker.ViewModels
{
	public class CreateProjectViewModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public List<TeamMemberViewModel>? Members { get; set; }

		public List<Member>? AppUsers { get; set; }

	}
}
