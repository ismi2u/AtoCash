using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using AtoCash.Data;

namespace AtoCash.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin")]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        [HttpPost]
        [ActionName("CreateRole")]

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

        public IActionResult ListUsers()
        {
            var users = userManager.Users;

            return Ok(users);
        }

        [HttpGet]
        [ActionName("ListRoles")]

        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;

            return Ok(roles);
        }

        [HttpGet("{id}")]
        [ActionName("GetUserByUserId")]
        public async Task<IActionResult> GetUserByUserId(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "User not Found" });
            }

            var rolenames = await userManager.GetRolesAsync(user);


            List<string> roleids = new List<string>();

            foreach (string role in rolenames)
            {
                var test = await roleManager.FindByNameAsync(role);

                roleids.Add(test.Id);
            }

            return Ok(new { user, roleids });
        }

        [HttpGet("{id}")]
        [ActionName("GetRoleByRoleId")]
        public async Task<IActionResult> GetRoleByRoleId(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Role not Found" });
            }

            return Ok(role);
        }

        [HttpDelete]
        [ActionName("DeleteRole")]

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Role not Found" });
            }

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
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user ==null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "User not Found" });
            }

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

        public async Task<IActionResult> AssignRole([FromBody] UserToRoleModel Model)
        {

            string userId = Model.UserId;
            ApplicationUser user = await userManager.FindByIdAsync(userId);

            //Remove Exisint Roles.
            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, roles.ToArray());
            //

            List<string> roleIds = Model.RoleIds;

            List<RespStatus> ListRespStatus = new List<RespStatus>();
            foreach (string RoleId in roleIds)
            {
                IdentityRole role = await roleManager.FindByIdAsync(RoleId);
                IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
                RespStatus respStatus;

                if (result.Succeeded)
                {
                    respStatus = new RespStatus()
                    {
                        Message = role.Name + " assigned to User", 
                        Status = "Success" 
                    };
                    ListRespStatus.Add(respStatus);
                }
                else
                {
                     respStatus = new RespStatus();
                    foreach (IdentityError error in result.Errors)
                    {
                        respStatus.Message = respStatus.Message + error.Description + "\n";
                        respStatus.Status = "Failure";
                    }
                    ListRespStatus.Add(respStatus);
                }
            }

            return Ok(ListRespStatus);


        }

    }
}