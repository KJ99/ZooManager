using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZooManager.Exceptions;
using ZooManager.Models.DTO.Requests;
using ZooManager.Models.DTO.Responses;
using ZooManager.Security.Authenticators;
using ZooManager.Security.Configs;

namespace ZooManager.Controllers
{
    [Route("v1/auth", Name = "auth_")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApiAuthenticator _authenticator;

        public AuthController(ApiAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        [HttpPost("login", Name = "login")]
        public async Task<ObjectResult> Login([FromBody] LoginRequest request)
        {
            ObjectResult result = null;
            try
            {
                string token = await _authenticator.GetJwtForCredentials(request);
                result = Ok(new LoginResponse { Token = token });
            } 
            catch(ApiException e)
            {
                result = BadRequest(new ErrorResponse { Code = e.Code, Message = e.Message });
            }
            
            return result;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin")]
        [HttpGet("foo", Name = "foo")]
        public StatusCodeResult Foo()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return NoContent();
        }


    }
}
