using System;
using AppoinmentScheduler.Services;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BusinessAppointment> BusinessAppointment { get; set; }
        public DbSet<BusinessService> BusinessService { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)   
        {
        }
        public bool CheckConnectionStatus()
        {
                return this.Database.CanConnect();   
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessAppointment>()
                .HasKey(b => b.BusinessAppointment_ID); 

            modelBuilder.Entity<BusinessService>()
                .HasKey(s => s.ServiceId);


            modelBuilder.Entity<BusinessService>()
                .HasOne(s => s.businessAppointment)
                .WithMany(b => b.BusinessService)
                .HasForeignKey(s => s.businessAppointmentID)
                .OnDelete(DeleteBehavior.Cascade);;
            
            modelBuilder.Entity<BusinessAppointment>()
                .HasOne(b => b.User) // BusinessAppointment has one User
                .WithOne(u => u.BusinessAppointment) // User can have many BusinessAppointments
                .HasForeignKey<BusinessAppointment>(b => b.Business_Account); // Business_Account is the foreign key
        }
    }    
}
