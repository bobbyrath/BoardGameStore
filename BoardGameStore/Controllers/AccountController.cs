using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BoardGameStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameStore.Controllers
{
    public class AccountController : Controller
    {
        SignInManager<BoardGameHubUser> _signInManager { get; set; }
        UserManager<BoardGameHubUser> _userManager { get; set; }
        private BoardGameHubDbContext _context { get; set; }

        public AccountController(SignInManager<BoardGameHubUser> signInManager, UserManager<BoardGameHubUser> userManager, BoardGameHubDbContext context)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        //Responds on GET Account/Register
        public IActionResult Register()
        {
            return View();
        }

        //Responds on POST Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BoardGameHubUser newUser = new BoardGameHubUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };

                IdentityResult creationResult = this._signInManager.UserManager.CreateAsync(newUser).Result;
                if (creationResult.Succeeded)
                {
                    IdentityResult passwordResult = this._signInManager.UserManager.AddPasswordAsync(newUser, model.Password).Result;
                    if (passwordResult.Succeeded)
                    {
                        this._signInManager.SignInAsync(newUser, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in passwordResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in creationResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }
            return View();
        }

        
        public IActionResult Logout()
        {
            this._signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}