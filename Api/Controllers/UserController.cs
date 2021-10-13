using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeXUserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        UserService userService;
        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User user = await userService.GetUserById(id);

            if (user == null)
            {
                return NotFound("Could not find user");
            }

            return Ok(user);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await userService.GetAllUsers();

            if (users == null)
            {
                return NotFound("Could not find users");
            }

            return Ok(users);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewUser()
        {
            User user = new User();
            user.Id = 1;
            user.FirstName = "Micheal";
            user.LastName = "test";

            try
            {
                await userService.AddUser(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("User added");
        }
    }
}
