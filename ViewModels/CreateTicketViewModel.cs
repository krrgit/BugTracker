﻿using BugTracker.Data.Enum;
using BugTracker.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.ViewModels
{
    public class CreateTicketViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? assignedDevId { get; set; }   
        public AppUser? AssignedDev { get; set; }
        public string? authorId { get; set; }
        public AppUser? Author { get; set; }
        public TicketPriority? Priority { get; set; }
        public TicketStatus? Status { get; set; }
        public TicketType? Type { get; set; }
		public int ProjectId { get; set; }
        public string? ProjectTitle { get; set; }

    }
}
