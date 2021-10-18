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
        EventService eventService;
        public UserController(UserService userService,EventService eventService)
        {
            this.userService = userService;
            this.eventService = eventService;
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> AddNewUser(User user)
        {
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


        [HttpDelete]
        public async Task<IActionResult> RemoveUser(int id)
        {
            try
            {
                await userService.RemoveUser(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("User Removed");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser(User userToUpdate)
        {
            User user = await userService.GetUserById(userToUpdate.Id);

            if (user == null)
            {
                return NotFound("Could not find user");
            }

            try
            {
                user.FirstName = userToUpdate.FirstName;
                user.LastName = userToUpdate.LastName;

                await userService.UpdateUser(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("User Updated");
        }


        [HttpGet("event")]
        public async Task<IActionResult> TestEvent(int id)
        {
            try
            {
               await eventService.PublishEvent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Event tested");
        }
    }
}
