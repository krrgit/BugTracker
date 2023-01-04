namespace BugTracker.Models
{
	public class ProjectMember
	{
		public string AppUserId { get; set; }
		public Member AppUser { get; set; }
		public int ProjectId { get; set; }
		public Project Project { get; set; }
	}
}
