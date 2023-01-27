using BugTracker.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
	public class Ticket
	{
		[Key]
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Member? AssignedDev { get; set; }
		public Member? Author { get; set; }
		public TicketPriority? Priority { get; set; }
		public TicketStatus? Status { get; set; }
		public TicketType? Type { get; set; }
		[ForeignKey("Project")]
		public int ProjectId { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

	}
}
