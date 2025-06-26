using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static void ConfigureCustomEntities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityType>().ToTable("ActivityTypes").HasIndex(a=>a.Code).IsUnique();
        modelBuilder.Entity<AppUser>().ToTable("Users");
        modelBuilder.Entity<CustomerConsumptions>().ToTable("CustomerConsumptions");
        modelBuilder.Entity<Customers>().ToTable("Customers");
        modelBuilder.Entity<Esdar>().ToTable("Esdar");
        modelBuilder.Entity<Tariff>().ToTable("Tariffs");
        modelBuilder.Entity<TariffSteps>().ToTable("TariffSteps");
        modelBuilder.Entity<IdentityUserLogin<string>>()
        .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(r => new { r.UserId, r.RoleId });

        modelBuilder.Entity<IdentityUserToken<string>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        modelBuilder.Entity<CustomerConsumptions>().HasOne<Customers>().
             WithMany(a=>a.Consumptions).HasPrincipalKey(a=>a.CustomerCode).HasForeignKey(a=>a.CustomerCode);
        

    }
}
