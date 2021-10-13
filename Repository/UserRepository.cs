using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository
    {
        readonly Context context;

        public UserRepository(Context context)
        {
            this.context = context;
        }
        public async Task<User> GetUserById(int id)
        {
            return await context.User.FindAsync(id);
        }


        public async Task<List<User>> GetAllUsers()
        {
            return await context.User.ToListAsync();
        }


        public async Task AddUser(User user)
        {
            await context.User.AddAsync(user);
            await context.SaveChangesAsync();
        }
    }
}
