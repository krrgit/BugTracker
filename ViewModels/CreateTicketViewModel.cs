﻿using BugTracker.Data.Enum;
using BugTracker.Models;

namespace BugTracker.ViewModels
{
	public class CreateTicketViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string? AssignedDevId { get; set; }
		public Member? AssignedDev { get; set; }
		public string? AuthorId { get; set; }
		public Member? Author { get; set; }
		public TicketPriority? Priority { get; set; }
		public TicketStatus? Status { get; set; }
		public TicketType? Type { get; set; }
		public int ProjectId { get; set; }
		public string? ProjectTitle { get; set; }
        public List<Member>? TeamSelectList { get; set; }
    }
}
