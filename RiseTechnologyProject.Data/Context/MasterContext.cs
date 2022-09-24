using Microsoft.EntityFrameworkCore;
using RiseTechnologyProject.Data.Models;
using RiseTechnologyProject.Data.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.Data.Context
{
    public class MasterContext : DbContext
    {
        public MasterContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(Resources.POSTRESQL_CONNECTION_STRING);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Contact>();
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
        public DbSet<User> Order { get; set; }
        public DbSet<Contact> Product { get; set; }
    }
}
