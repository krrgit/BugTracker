﻿using BugTracker.Models;

namespace BugTracker.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Ticket>? Tickets { get; set; }
        public List<AppUser>? Members { get; set; }

        public List<AppUser>? AppUsers { get; set; }

    }
}