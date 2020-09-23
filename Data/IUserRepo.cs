using ASPNETCoreWebApi.DTOs;
using ASPNETCoreWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApi.Data
{
    public interface IUserRepo
    {
        IEnumerable<UserReadDTO> GetAllUser(int page);
       // UserReadDTO GetUser(string email);
        Task<IdentityResult> UpdateUser(UpdateDTO model, UserModel user);
        Task<IdentityResult> DeleteUser(UserModel user);
    }
}
