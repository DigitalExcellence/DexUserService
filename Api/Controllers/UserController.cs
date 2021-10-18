using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Http;
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
        public UserController(UserService userService, EventService eventService)
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
                return Ok("event sucessfully tested");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


        [HttpPost("event")]
        public async Task<IActionResult> Event()
        {
            HttpRequest req = HttpContext.Request;
            //log.LogInformation("C# HTTP trigger function processed a request.");
            string response = string.Empty;
            BinaryData events = await BinaryData.FromStreamAsync(req.Body);
            //log.LogInformation($"Received events: {events}");

            EventGridEvent[] eventGridEvents = EventGridEvent.ParseMany(events);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                // Handle system events
                if (eventGridEvent.TryGetSystemEventData(out object eventData))
                {
                    // Handle the subscription validation event
                    if (eventData is SubscriptionValidationEventData subscriptionValidationEventData)
                    {
                        // Do any additional validation (as required) and then return back the below response

                        var responseData = new SubscriptionValidationResponse()
                        {
                            ValidationResponse = subscriptionValidationEventData.ValidationCode
                        };
                        return new OkObjectResult(responseData);
                    }
                    // Handle the storage blob created event
                    else if (eventData is StorageBlobCreatedEventData storageBlobCreatedEventData)
                    {
                    }
                }
                // Handle the custom contoso event
                else if (eventGridEvent.EventType == "userUpdated")
                {
                    //var contosoEventData = eventGridEvent.Data.ToObjectFromJson<ContosoItemReceivedEventData>();
                    User user = new User();
                    user.FirstName = "event fetched :D";
                    user.LastName = "event fetched :D";

                    await userService.AddUser(user);
                }
            }

            return new OkObjectResult("Could not fetch event");
        }
    }
}
    
