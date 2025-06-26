using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataBase
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext>op) : IdentityDbContext<AppUser>(op)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureCustomEntities();
        }
    }
}
