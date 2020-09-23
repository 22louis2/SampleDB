using ASPNETCoreWebApi.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApi.Data
{
    public interface IAccountRepo
    {
        Task<IdentityResult> Register(RegisterDTO model);
        Task<string> Login(LoginDTO model);
    }
}
