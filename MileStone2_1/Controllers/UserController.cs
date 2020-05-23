using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MileStone2_1.Models;
using Microsoft.AspNetCore.Authentication;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MileStone2_1.Controllers

{
    
    public class UserController : Controller
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

        }
       


        [HttpPost]
        public async Task <IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Phones");

        }

       [HttpGet]
   
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        
        public async  Task <IActionResult> Register(User model)
        {
            if(ModelState.IsValid)
            { 
              
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
              var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if(signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Phones");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
      
        public IActionResult Login()
        {
            return View();
        }

       

        [HttpPost]
       
        public async Task<IActionResult> Login(Login model, string ReturnUrl)
        {

           
            if (ModelState.IsValid)
            {

               
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);

                    }
                    else
                    {
                        return RedirectToAction("Index", "Phones");
                    }

                      

                }

               
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                
           }

            return View(model);
        }

    }
}
