using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
	public class Project
	{
		[Key]
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }

		public ICollection<AppUserProject> ProjectMembers { get; set; }
		public ICollection<Ticket> Tickets { get; set; }
	}
}
