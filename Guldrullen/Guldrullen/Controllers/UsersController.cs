using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Guldrullen.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Guldrullen.Controllers
{
    public class UsersController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        IdentityDbContext context;

        public UsersController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IdentityDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsersRegisterViewModel model)
        {
            #region Validate viewmodel
            if (!ModelState.IsValid)
                return View(model);
            #endregion

            #region Create user
            await context.Database.EnsureCreatedAsync();

            var result = await userManager.CreateAsync(new IdentityUser(model.UserName), model.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("UserName", result.Errors.First().Description);
                return View(model);
            }
            #endregion

            #region Log in and redirect

            await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            return Redirect("/Movies/Index");

            #endregion
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsersRegisterViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (!result.Succeeded)
            {
                return View(model);
            }
                return Redirect("/Movies/Index");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Redirect("/Movies/Index");
        }

        public IActionResult CheckIfUserIsSignedIn(int id)
        {
            var user = new UsersRegisterViewModel();

            user.UserName = userManager.GetUserName(HttpContext.User);

            if (id == 1)
            {
                return PartialView("DisplaySignUp", user);
            }
            else
            {
                return PartialView("DisplaySignIn", user);
            }
        }
    }
}
