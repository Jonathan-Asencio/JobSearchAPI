using Microsoft.EntityFrameworkCore;
using putavettoworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<JobSearch> JobSearch { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
    }
}
