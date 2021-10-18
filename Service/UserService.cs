using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService
    {
        UserRepository userRepository;
        EventService eventService;
        public UserService(UserRepository userRepository, EventService eventService)
        {
            this.userRepository = userRepository;
            this.eventService = eventService;
        }

        public async Task<User> GetUserById(int id)
        {
            return await userRepository.GetUserById(id);
        }


        public async Task<List<User>> GetAllUsers()
        {
            return await userRepository.GetAllUsers();
        }

        public async Task AddUser(User user)
        {
           await userRepository.AddUser(user);
           await eventService.PublishEvent("userAdded", user.Id);
        }


        public async Task UpdateUser(User user)
        {
            await userRepository.UpdateUser(user);
            await eventService.PublishEvent("userUpdated", user.Id);
        }

        public async Task RemoveUser(int id)
        {
            await userRepository.RemoveUser(id);
            await eventService.PublishEvent("userRemoved", id);
        }
    }
}
