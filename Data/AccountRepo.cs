using ASPNETCoreWebApi.DTOs;
using ASPNETCoreWebApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETCoreWebApi.Data
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _config;

        public AccountRepo(UserManager<UserModel> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<string> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result) return null;

            // 1. Create claims for JWT
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.LastName)
                };

            // 2. Get JWT Secret Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            // 3. Generate the Signing Credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // 4. Create Security Token Descriptor
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds
            };

            // 5. Build Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 6. Create the token
            var token = tokenHandler.CreateToken(securityTokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<IdentityResult> Register(RegisterDTO model)
        {
            var user = new UserModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                Photo = model.Photo
            };

            return await _userManager.CreateAsync(user, model.Password);
        }
    }
}
