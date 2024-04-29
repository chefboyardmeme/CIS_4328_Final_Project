using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
//
using UNFBusShuttle.Models;

namespace UNFBusShuttle.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=OspreyTransit;Trusted_Connection=True;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverLocation> DriverLocations { get; set; }        
        public DbSet<BusSchedule> BusSchedules { get; set; }        
        public DbSet<RiderRequest> RiderRequests { get; set; }
    }

}
