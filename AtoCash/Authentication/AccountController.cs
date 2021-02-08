using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AtoCash.Authentication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        // GET: api/<AccountController>
        [HttpPost]
        [ActionName("Register")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            //check if email is already in use if yes.. throw error

            var useremail = await userManager.FindByEmailAsync(model.Email);

            if (useremail != null)
            {
                return NotFound(new ReponseStatus { Status = "Failure", Message = "Email is already taken" });
            }

            MailAddress mailAddress = new MailAddress(model.Email);
            
            //MODIFY HOST DOMAIN NAME HERE => CURRENTLY only GMAIL and MAILINATOR
            if ( (mailAddress.Host.ToUpper() != "MAILINATOR.COM") && mailAddress.Host.ToUpper() != "GMAIL.COM")
            {
                return NotFound(new ReponseStatus { Status = "Failure", Message = "Use company mail address!" });
            }
            //Creating a IdentityUser object
            var user = new ApplicationUser { UserName= model.Username, Email = model.Email, PasswordHash = model.Password };

             IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new ReponseStatus { Status = "Success", Message = "User Registered Successfully" });
            }

            ReponseStatus respStatus = new ReponseStatus();

            foreach (IdentityError error in result.Errors)
            {
                respStatus.Message = respStatus.Message + error.Description + "\n";
            }

            return BadRequest(respStatus);

        }


        [HttpPost]
        [ActionName("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //Creating a IdentityUser object

            var user = await userManager.FindByEmailAsync(model.Email);

            var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            
            //if signin successful send message
            if (result.Succeeded)
            {
                var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey12323232"));

                var signingcredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

                var modeluser = await userManager.FindByEmailAsync(model.Email);
                var userroles = await userManager.GetRolesAsync(modeluser);
                //add claims
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, modeluser.UserName),
                 new Claim(ClaimTypes.Email, model.Email)
                };
                //add all roles belonging to the user
                 foreach (var role in userroles) 
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var tokenOptions = new JwtSecurityToken(

                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddHours(5),
                     signingCredentials: signingcredentials
                    ) ;


                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                
                return Ok( new { Token = tokenString });
            }

            return Unauthorized(new ReponseStatus { Status = "Failure", Message = "Username or Password Incorrect" });
        }



    }
}
