using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private SymmetricSecurityKey _jwtKey;
        public JwtService(IConfiguration config)
        {
            _config = config;
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));

        }
        public string CreateJwt(User user, IList<string> userRole)
        {
            List<Claim> userClaims = GetClaim(user, userRole);
            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_config["JWT:ExpiresInDays"])),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }

        #region Private Helper Method
        private List<Claim> GetClaim(User user, IList<string> userRole)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        #endregion
    }
}
