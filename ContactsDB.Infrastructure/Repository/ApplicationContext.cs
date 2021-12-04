using Microsoft.EntityFrameworkCore;
using ContactsDB.Domain.Models;
using System.Configuration;


namespace ContactsDB.Infrastructure.Repository
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Zipcode> Zipcodes { get; set; }

        public DbSet<Contact> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["post"].ConnectionString);            
        }
    }
}
