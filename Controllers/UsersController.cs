using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZooManager.Data.Contexts;
using ZooManager.Models.DTO.Requests;
using ZooManager.Models.DTO.Responses;

namespace ZooManager.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UsersController : ZooApiController
    {
        public UsersController(IMapper mapper, ApiDbContext db, UserManager<IdentityUser> userManager) : base(mapper, db, userManager)
        {
        }

        [HttpGet("")]
           
        public async Task<ObjectResult> List()
        {
            IList<IdentityUser> admins = await _userManager.GetUsersInRoleAsync("api.admin");
            IList<IdentityUser> managers = await _userManager.GetUsersInRoleAsync("zoo.manager");
            IList<IdentityUser> employees = await _userManager.GetUsersInRoleAsync("zoo.employee");

            List<IdentityUser> allUsers = new List<IdentityUser>();

            allUsers.AddRange(admins);
            allUsers.AddRange(managers);
            allUsers.AddRange(employees);

            List<UserResponse> mapped = new List<UserResponse>();

            foreach(IdentityUser user in allUsers)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                UserResponse mappedUser = Map<UserResponse>(user);
                mappedUser.Roles = roles;
                mapped.Add(mappedUser);
            }

            return Ok(mapped);
        }

        [HttpPost("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin")]
        public async Task<ActionResult> Post([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ProcessValidationViolations();
            }
            
            IdentityUser user = Map<IdentityUser>(request);
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);

            await _userManager.CreateAsync(user);
            await _userManager.AddToRolesAsync(user, request.Roles);

            user = await _userManager.FindByIdAsync(user.Id);
            UserResponse mapped = Map<UserResponse>(user);
            mapped.Roles = await _userManager.GetRolesAsync(user);

            return Created("", mapped);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        public async Task<ActionResult> Get([FromRoute] string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if(user == null) 
            {
                return NotFound();
            }

            UserResponse mapped = Map<UserResponse>(user);
            mapped.Roles = await _userManager.GetRolesAsync(user);

            return Ok(mapped);
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin")]
        public async Task<ActionResult> Patch([FromRoute] string id, [FromBody] UserUpdateRequest request)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }
            if(!ModelState.IsValid)
            {
                return ProcessValidationViolations();
            }

            user.UserName = request.Username;
            user.NormalizedUserName = request.Username.ToUpper();
            user.Email = request.Email;
            user.NormalizedEmail = request.Email.ToUpper();

            await _userManager.AddToRolesAsync(user, request.RolesToGrant);
            await _userManager.RemoveFromRolesAsync(user, request.RolesToRevoke);

            user = await _userManager.FindByIdAsync(id);
            UserResponse mapped = Map<UserResponse>(user);
            mapped.Roles = await _userManager.GetRolesAsync(user);

            return Accepted(mapped);
        }

        

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin")]
        public async Task<StatusCodeResult> Delete([FromRoute] string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            
            return NoContent();
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("password")]
        public async Task<ActionResult> UpdatePassword([FromBody] PasswordUpdateRequest request)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            IdentityUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return ProcessValidationViolations();
            }
            if(!(await _userManager.CheckPasswordAsync(user, request.CurrentPassword)))
            {
                return BadRequest(new ValidationErrorResponse[] { new() { Path = "currentPassword", Message = "Current password is not valid" } });
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
            await _userManager.UpdateAsync(user);

            return NoContent();
        }


        [HttpPatch("{id}/password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin")]
        public async Task<ActionResult> UpdatePasswordAdmin([FromRoute] string id, [FromBody] AdminPasswordUpdateRequest request)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return ProcessValidationViolations();
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

    }
}
