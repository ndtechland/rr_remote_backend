using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System;
using RR_Remote.Models.Entity;

namespace RR_Remote.Context
{
    public class AppDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AdminLogin> AdminLogins { get; set; }
        public DbSet<CategoryMaster> CategoryMasters { get; set; }
        public DbSet<BannerMaster> BannerMasters { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
