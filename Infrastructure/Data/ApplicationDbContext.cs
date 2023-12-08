using Infrastructure.Utility;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Template> Template { get; set; }
        public DbSet<TemplateGroup> TemplateGroup { get; set; }
        public DbSet<UserTemplate> UserTemplate { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This is where you can enter seed data
            base.OnModelCreating(modelBuilder);

            //Seed Template Groups
            //modelBuilder.Entity<TemplateGroup>().HasData(
            //    new TemplateGroup
            //    {
            //        Id=1,
            //        Name = "Gym"
            //    },
            //     new TemplateGroup
            //     {
            //         Id = 2,
            //         Name = "Intelligence"
            //     }
            //);
        }


    }
}



