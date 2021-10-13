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
        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
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
        }
    }
}
