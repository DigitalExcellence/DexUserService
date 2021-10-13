using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
        : base(options)
        {
        }
        public DbSet<User> User { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new User { FirstName = "John", lastName = "Smith" });
            modelBuilder.Entity<User>().HasData(new User { FirstName = "Bob", lastName = "Bobbers" });
            modelBuilder.Entity<User>().HasData(new User { FirstName = "Alice", lastName = "Garcia" });
        }
    }
}
