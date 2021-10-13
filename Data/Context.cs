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
            modelBuilder.Entity<User>().HasData(new User { Id = 1, FirstName = "John", LastName = "Smith" });
            modelBuilder.Entity<User>().HasData(new User { Id = 2, FirstName = "Bob", LastName = "Bobbers" });
            modelBuilder.Entity<User>().HasData(new User { Id = 3, FirstName = "Alice", LastName = "Garcia" });
        }
    }
}
