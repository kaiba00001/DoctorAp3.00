using DoctorAp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoctorAp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BookingLeadEntity> BookingLead { get; set; }


        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
