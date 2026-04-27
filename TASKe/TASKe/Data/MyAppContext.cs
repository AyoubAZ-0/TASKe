using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TASKe.Models;

namespace TASKe.Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Taskitem> Tasks { get; set; }

    }
}
