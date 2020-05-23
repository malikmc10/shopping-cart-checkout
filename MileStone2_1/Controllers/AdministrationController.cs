
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MileStone2_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MileStone2_1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public  async Task<IActionResult> DeleteUser (string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");

            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");

            }
            else
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListRoles");
            }
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(String id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot";
                return View("NotFound");
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserView
            {
                UserId = user.Id,
                Email = user.Email,
                Name = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserView model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.Name;

                var result = await userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListUsers");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View(model);
            }
           
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRole(CreateRoleView model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityrole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityrole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleView
            {
                Id = role.Id,
                RoleName = role.Name
            };



            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }


            }
            return View(model);

        }



        // This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditRole(EditRoleView model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");

            }

            var model = new List<UserRoleView>();

            foreach (var user in userManager.Users.ToList())
            {
                var userRoleView = new UserRoleView
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleView.IsSelected = true;
                }
                else
                {
                    userRoleView.IsSelected = false;
                }


                model.Add(userRoleView);

            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleView> model, string roleId)
        {

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {roleId} cannot be found";
                return View("NotFound");
            }
           for (int i = 0; i < model.Count; i++)
            {
              var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;
                if (model[i].IsSelected && ! await (userManager.IsInRoleAsync(user, role.Name)))
                    
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);

                }
                else if (!model[i].IsSelected && await (userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }

            }


            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
