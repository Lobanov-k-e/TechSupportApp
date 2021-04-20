using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechSupportApp.Infrastructure.Identity;
using TechSupportApp.WebApi.ViewModels.Account;

namespace TechSupportApp.WebApi.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public AccountController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() {ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(model.ReturnUrl ?? "/Admin");
                    }
                    ModelState.AddModelError("", "Incorrect login or password");
                }               
            }
            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Login");
        }
    }
}