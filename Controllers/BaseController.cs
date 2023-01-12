using BugTracker.Data;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BugTracker.Controllers
{
    public class BaseController : Controller
    {
        public List<ProjectLinkViewModel> _projecLinkVM { get; set; }
        protected readonly AppDBContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected string userId;
        Member user;

        public BaseController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!User.Identity.IsAuthenticated) return;

            // Get Project List for User
            userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            user = _context.AppUsers.FirstOrDefaultAsync(i => i.Id == userId).Result;

            ViewData["FullName"] = user.FullName();


            var projects =  _context.Projects
                .Include(p => p.ProjectMembers)
                .Where(pm => pm.ProjectMembers.Any(m => m.AppUserId == userId)).ToList();

            _projecLinkVM = new List<ProjectLinkViewModel>();
            foreach (var project in projects)
            {
                var p = new ProjectLinkViewModel
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                };
                _projecLinkVM.Add(p);
            }
            ViewBag.ProjecLinkVM = _projecLinkVM;
            base.OnActionExecuting(context);
        }
    }
}
