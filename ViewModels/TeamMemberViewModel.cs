using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracker.ViewModels
{
    public class TeamMemberViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

    }
}
