using ASPNETCoreWebApi.DTOs;
using ASPNETCoreWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApi.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _config;
        public UserRepo(UserManager<UserModel> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public IEnumerable<UserReadDTO> GetAllUser(int page)
        {

            // Get the users details from the database
            var model = _userManager.Users.Skip(5 * (page - 1)).Take(5);

            // Declare a list to be returned
            var users = new List<UserReadDTO>();

            // Reshape the users details to the DTO model
            foreach (var item in model)
            {
                // Construct the DTO values here
                var user = new UserReadDTO
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    Photo = item.Photo,
                    DateCreated = item.DateCreated
                };

                // Add to the list to return
                users.Add(user);
            }

            return users;
        }

        public async Task<IdentityResult> UpdateUser(UpdateDTO model, UserModel user)
        {
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.Photo = model.Photo ?? user.Photo;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUser(UserModel user)
        {            
            return await _userManager.DeleteAsync(user);
        }

    }
}
