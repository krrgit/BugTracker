using BugTracker.Data;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.ViewModels
{
    public class ManageMembersViewModel
    {
        public IEnumerable<TeamMemberViewModel> Members { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }

    }
}
