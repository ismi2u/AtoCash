using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AtoCash.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.roleManager = roleManager;

            this.userManager = userManager;

            this.signInManager = signInManager;
        }


        [HttpPost]
        [ActionName("CreateRole")]
        //[Authorize(Roles = "AtominosAdmin, Admin, User, Manager, Finmanager")]
        public async Task<IActionResult> CreateRole([FromBody] RoleModel model)
        {
            


            IdentityRole identityRole = new IdentityRole()
            {
                Name = model.RoleName
            };

            IdentityResult result = await roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return Ok(new RespStatus { Status = "Success", Message = "New Role Created" });
            }

            RespStatus respStatus = new RespStatus();

            foreach (IdentityError error in result.Errors)
            {
                respStatus.Message = respStatus.Message + error.Description + "\n";
            }

            return BadRequest(respStatus);
        }



        [HttpGet]
        [ActionName("ListUsers")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;

            return Ok(users);
        }



        [HttpGet]
        [ActionName("ListRoles")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;

            return Ok(roles);
        }

        [HttpDelete]
        [ActionName("DeleteRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            IdentityResult result = await roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok(new RespStatus { Status = "Success", Message = "Role Deleted" });
            }

            RespStatus respStatus = new RespStatus();

            foreach (IdentityError error in result.Errors)
            {
                respStatus.Message = respStatus.Message + error.Description + "\n";
            }

            return NotFound(respStatus);
        }

        [HttpDelete]
        [ActionName("DeleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            IdentityResult result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok(new RespStatus { Status = "Success", Message = "User Deleted" });
            }

            RespStatus respStatus = new RespStatus();

            foreach (IdentityError error in result.Errors)
            {
                respStatus.Message = respStatus.Message + error.Description + "\n";
            }

            return NotFound(respStatus);
        }




        [HttpPut]
        [ActionName("EditRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(EditRoleModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            role.Name = model.RoleName;

            IdentityResult result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok(new RespStatus { Status = "Success", Message = "Role Updated" });
            }

            RespStatus respStatus = new RespStatus();

            foreach (IdentityError error in result.Errors)
            {
                respStatus.Message = respStatus.Message + error.Description + "\n";
            }

            return NotFound(respStatus);
        }


        [HttpPut]
        [ActionName("EditUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            user.UserName = model.Username;

            IdentityResult result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new RespStatus { Status = "Success", Message = "User Updated" });
            }

            RespStatus respStatus = new RespStatus();

            foreach (IdentityError error in result.Errors)
            {
                respStatus.Message = respStatus.Message + error.Description + "\n";
            }

            return NotFound(respStatus);
        }



        ///Assign Role to User 
        /// One by One
        ///
        [HttpPost]
        [ActionName("AssignRole")]
        //[Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> AssignRole([FromBody] UserToRoleModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            var role = await roleManager.FindByIdAsync(model.RoleId);

            if (await userManager.IsInRoleAsync(user, role.Name))
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "User already has the Role" });
            }


            IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return Ok(new RespStatus { Status = "Success", Message = role.Name + " assigned to User" });
            }

            RespStatus respStatus = new RespStatus();

            foreach (IdentityError error in result.Errors)
            {
                respStatus.Message = respStatus.Message + error.Description + "\n";
            }

            return BadRequest(respStatus);

        }


    }
}