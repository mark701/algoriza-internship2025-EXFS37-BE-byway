using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implemnetion
{
    public class AuthService : IAuthService
    {

        private readonly IHttpContextAccessor _access;
        private readonly string PathString = "Assets/Users";
        private readonly IConfiguration _config;

        public AuthService(IConfiguration configuration, IHttpContextAccessor access)
        {

            _access = access;
            _config = configuration;

        }

        public void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }

        }

        public string GenerateAdminToken(Admins admins)
        {
            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.Name, admins.adminName),
                new Claim(ClaimTypes.Email, admins.adminEmail),
                new Claim(ClaimTypes.NameIdentifier, admins.adminID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var Token = GenerateJwtToken(claims);

            return Token;

        }

        public string GenerateUserToken(Users users)
        {
            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.Name, users.UserName),
                new Claim(ClaimTypes.Email, users.UserEmail),
                new Claim(ClaimTypes.NameIdentifier, users.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var Token = GenerateJwtToken(claims);

            return Token;
        }

        public string GenerateJwtToken (List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string? GetClaim(string claimType)
        {
            try
            {
                var user = _access.HttpContext?.User;
                if (user == null || !user.Identity.IsAuthenticated)
                    return null; 

                var claim = user.FindFirst(claimType);
                return claim?.Value;
            }
            catch
            {
                return null;
            }
        }


        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt)))
            {
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return computedHash == storedHash;
            }
        }
    }
}
