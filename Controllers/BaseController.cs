using BugTracker.Data;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    public class BaseController : Controller
    {
        public List<ProjectLinkViewModel> _projecLinkVM { get; set; }
        protected readonly AppDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!User.Identity.IsAuthenticated) return;

            string id = _httpContextAccessor.HttpContext?.User.GetUserId();
            var projects =  _context.Projects
                .Include(p => p.ProjectMembers)
                .Where(pm => pm.ProjectMembers.Any(m => m.AppUserId == id)).ToList();

            _projecLinkVM = new List<ProjectLinkViewModel>();
            foreach (var project in projects)
            {
                var p = new ProjectLinkViewModel
                {
                    Id = project.Id,
                    Title = project.Title
                };
                _projecLinkVM.Add(p);
            }
            ViewBag.ProjecLinkVM = _projecLinkVM;
            base.OnActionExecuting(context);
        }
    }
}
