using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASPNETCoreWebApi.Data;
using ASPNETCoreWebApi.DTOs;
using ASPNETCoreWebApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASPNETCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAccountRepo _account;
        public AccountController(ILogger<UserController> logger, IAccountRepo account)
        {
            _logger = logger;
            _account = account;
        }

        // GET: api/<AccountController>/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if(ModelState.IsValid)
            {
                var result = await _account.Register(model);

                if (result == null) return BadRequest("User cannot be registered. Invalid details provided");

                if (result.Succeeded) return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        // GET: api/<AccountController>/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if(ModelState.IsValid)
            {
                var result = await _account.Login(model);

                if (result == null) return BadRequest("Invalid Password or Email.");

                return Ok(new { result });
            }

            return BadRequest("Invalid Email or Password");
        }
    }
}
