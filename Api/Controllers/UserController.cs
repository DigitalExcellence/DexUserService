using Azure;
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

            return Ok(user);
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

        /// <summary>
        /// publish a test event with your chosen topic and data
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpGet("event")]
        public async Task<IActionResult> TestEvent(string topic, int subjectId)
        {
            try
            {
               Response response =  await eventService.PublishEvent(topic,subjectId);
                if (response.Status.Equals(200))
                {
                    return Ok("event sucessfully tested");
                }
                else
                {
                    return BadRequest("Could not test/publish event");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        /// <summary>
        /// WebHook endpoint for Azure Event Grid
        /// </summary>
        /// <returns></returns>
        [HttpPost("event")]
        public async Task<IActionResult> Event()
        {
            HttpRequest req = HttpContext.Request;
            string response = string.Empty;
            BinaryData events = await BinaryData.FromStreamAsync(req.Body);

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
                }
                // Handle the userUpdated event
                else if (eventGridEvent.EventType == "userUpdated")
                {
                    //var contosoEventData = eventGridEvent.Data.ToObjectFromJson<ContosoItemReceivedEventData>();
                    User user = new User();
                    user.FirstName = "userUpdated";
                    user.LastName = "userUpdated";

                    await userService.AddUser(user);
                }

                // Handle the userRemoved event
                else if (eventGridEvent.EventType == "userRemoved")
                {
                    //var contosoEventData = eventGridEvent.Data.ToObjectFromJson<ContosoItemReceivedEventData>();
                    User user = new User();
                    user.FirstName = "userRemoved";
                    user.LastName = "userRemoved";

                    await userService.AddUser(user);
                }

                // Handle the userRemoved event
                else if (eventGridEvent.EventType == "userAdded")
                {
                    //var contosoEventData = eventGridEvent.Data.ToObjectFromJson<ContosoItemReceivedEventData>();
                    User user = new User();
                    user.FirstName = "userAdded";
                    user.LastName = "userAdded";

                    await userService.AddUser(user);
                }
            }

            return new OkObjectResult("Could not fetch event");
        }
    }
}
    
