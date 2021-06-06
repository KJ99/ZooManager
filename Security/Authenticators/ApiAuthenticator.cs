using Microsoft.AspNetCore.Identity;
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
using ZooManager.Security.Configs;

namespace ZooManager.Security.Authenticators
{

    public class ApiAuthenticator
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public ApiAuthenticator(UserManager<IdentityUser> userManager, JwtConfig jwtConfig)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig;
        }

        public async Task<string> GetJwtForCredentials(LoginRequest request)
        {

            IdentityUser user = await _userManager.FindByNameAsync(request.Username);
            if(user == null || !(await _userManager.CheckPasswordAsync(user, request.Password)))
            {
                throw new ApiException("AUTH.001", "Bad credentials. Ensure that your username and password are valid");
            }
            
            return await GenerateJwtToken(user);
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            
            SecurityTokenDescriptor tokenDescriptor = await BuildDescriptor(user);

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        private async Task<SecurityTokenDescriptor> BuildDescriptor(IdentityUser user)
        {
            Claim[] claims = await BuildClaims(user);
            byte[] tokenSecret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            SymmetricSecurityKey securityKey = new(tokenSecret);

            return new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature)
            };
        }

        private async Task<Claim[]> BuildClaims(IdentityUser user)
        {
            List<Claim> claims = new();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            foreach(string role in (await _userManager.GetRolesAsync(user)))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims.ToArray();
        }
    }
}
