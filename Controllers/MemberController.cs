using BugTracker.Data;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    public class MemberController : BaseController
    {
        public MemberController(AppDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<IActionResult> Manage()
        {
            if (!User.IsInRole("admin"))
            {
                var model = new ErrorViewModel (){ RequestId = "Restricted Page"};
                return View("Error", model);
            }

            var members = await _context.Users.ToListAsync();
            var membersVM = new List<TeamMemberViewModel>();
            var roles = await _context.Roles.ToListAsync();

            foreach (var m in members) {
                string role = "submitter";
                try
                {
                    var roleId = (await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == m.Id)).RoleId;
                    role = (await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId)).Name;
                }
                catch (Exception ex)
                {
                    role = "submitter";
                }

                var member = new TeamMemberViewModel()
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Role = role
                };
                membersVM.Add(member);
            }

            var manageVM = new ManageMembersViewModel()
            {
                Members = membersVM,
                Roles = roles
            };

            return View(manageVM);
        }

        [HttpPost]
        public async Task<IActionResult> SetRole(string UserId, string RoleId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Manage");
            }

            var role = await _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == UserId);
            if (role != null)
            {
                _context.UserRoles.Remove(role);
                _context.SaveChanges();

                var userRole = new IdentityUserRole<string>()
                {
                    UserId = UserId,
                    RoleId = RoleId
                };

                _context.UserRoles.Add(userRole);
                _context.SaveChanges();
            }

            return RedirectToAction("Manage");
        }


        [HttpPost]
        public async Task<IActionResult> Remove(string userId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Manage");
            }
            
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Manage");
        }
    }
}
