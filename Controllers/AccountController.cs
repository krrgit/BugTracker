using BugTracker.Data;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        public AccountController(UserManager<Member> userManager, SignInManager<Member> signInManager, AppDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Submitter);
            } else
            {
                TempData["Error"] = "failed to make user";
                return View(registerVM);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            EditAccountViewModel vm = new EditAccountViewModel() { 
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.Email
            };

			return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId);

            if (user == null)
            {
                return View(vm);
            }

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Email = vm.EmailAddress;

            _context.Update(user);
            _context.SaveChanges();

            return RedirectToAction("Index","Dashboard");
        }
    }
}
