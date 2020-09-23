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
using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepo _user;
        private readonly UserManager<UserModel> _userManager;

        public UserController(ILogger<UserController> logger, IUserRepo user, UserManager<UserModel> userManager)
        {
            _logger = logger;
            _user = user;
            _userManager = userManager;
        }

        // GET: api/<UserController> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{page}")]
        public IActionResult GetAllUser(int page = 1)
        {
            if(ModelState.IsValid)
            {
                if (page < 1) return BadRequest("Invalid Page Format");

                var users = _user.GetAllUser(page);

                if (users == null) return BadRequest("Users does not exist");

                if (users.Count() == 0) return Ok("Page does not exists");

                // Return to http response
                return Ok(users);
            }
            return BadRequest("Invalid Request");
        }

        // GET: api/<UserController> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser()
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null) return BadRequest("User does not exist");

                // Reshape the users details to the DTO model
                var userReturned = new UserReadDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Photo = user.Photo,
                    DateCreated = user.DateCreated
                };

                // Return to http response
                return Ok(userReturned);
            }
            return BadRequest("Invalid Request");
        }

        // PATCH: api/<UserController> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateDTO model)
        {
            if(ModelState.IsValid)
            {
                var userCheck = await _userManager.GetUserAsync(User);

                var user = await _user.UpdateUser(model, userCheck);

                if (user == null) return NotFound();

                if (user.Succeeded)
                {
                    return Ok("Updated Successfully");
                }
                else
                {
                    return BadRequest(user.Errors);
                }
            }
            return BadRequest("Invalid Details");
        }

        // PATCH: api/<UserController> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.GetUserAsync(User);

                var user = await _user.DeleteUser(userCheck);

                if (user == null) return NotFound();

                if (user.Succeeded)
                {
                    return Ok("Deleted Successfully");
                }
                else
                {
                    return BadRequest(user.Errors);
                }
            }
            return BadRequest("Invalid Details");
        }
    }
}

