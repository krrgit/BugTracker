using BugTracker.Data.Enum;
using BugTracker.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.ViewModels
{
	public class TicketViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Member? AssignedDev { get; set; }
		public Member? Author { get; set; }
		public TicketPriority? Priority { get; set; }
		public TicketStatus? Status { get; set; }
		public TicketType? Type { get; set; }

		public string? ProjectTitle { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
