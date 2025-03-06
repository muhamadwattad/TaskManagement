using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.BusinessLogic.BL.DTOs;
using TaskManagement.DataAccessLayer.Entities;
using TaskManagement.Framework.Exceptions;
using static TaskManagement.Framework.Statics.UserStatics;

namespace TaskManagement.BusinessLogic.BL
{
    public class AuthBL
    {

        private readonly List<User> Users = [
            new User("Muhamad Wattad","Muhamad","123456789",UserRoles.Admin),
            new User("Admin","Admin","123456789",UserRoles.User)];


        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;


        public AuthBL(IConfiguration configuration)
        {
            // Getting Jwt Settings from AppSettings - based on Env (Production, Development)
            _key = configuration["JwtSettings:Key"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
        }

        public string Login(AuthDTOs.Request.Login dto)
        {
            //Instead of this code, it should be AWS Cognito User Pool code.
            var user = Users.FirstOrDefault(s => s.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase) && s.Password == dto.Password);
            return user is null ? throw new ValidationException("Username or password is incorrect") : GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,user.Id.ToString())
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
