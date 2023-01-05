using BugTracker.Data;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        private readonly AppDBContext _context;
        public AccountController(UserManager<Member> userManager, SignInManager<Member> signInManager, AppDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            // Look for User
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user == null)
            {
                TempData["Error"] = "Wrong credentials. Please, try again";
                return View(loginVM);
            }

            // User Found; Check Password
            var correctPassword = await _userManager.CheckPasswordAsync(user, loginVM.Password);
            if (!correctPassword)
            {
                // LoginVM.CorrectPassword?
                TempData["Error"] = "Wrong credentials. Please, try again";
                return View(loginVM);
            }

            // User & Password correct, try sign in
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Error signing in";
                return View(loginVM);
            }

            return RedirectToAction("Index", "Project");
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);

            // User exists
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new Member()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Submitter);
            }

            return RedirectToAction("Index", "Project");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
