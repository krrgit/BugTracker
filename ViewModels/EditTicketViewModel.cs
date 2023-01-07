using BugTracker.Data.Enum;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracker.ViewModels
{
	public class EditTicketViewModel
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? AssignedDevId { get; set; }
		public Member? AssignedDev { get; set; }
		public Member? Author { get; set; }
		public string? AuthorId { get; set; }
		public TicketPriority? Priority { get; set; }
		public TicketStatus? Status { get; set; }
		public TicketType? Type { get; set; }
		public int ProjectId { get; set; }
		public string? ProjectTitle { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public List<Member>? TeamSelectList { get; set; }
	}
}
