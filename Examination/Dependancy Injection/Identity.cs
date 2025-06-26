using DAL.DataBase;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
namespace Examination.Dependency_Injection
{
    public static class Identity
    {
        public static IServiceCollection AddIdentityDependencyInjection(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(a =>
            {
                a.Password.RequiredLength = 8;
                a.Password.RequireUppercase = true;
                a.Password.RequireLowercase = true;
                a.Password.RequireDigit = true;
                a.Password.RequiredUniqueChars =0;
                a.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }
    }
}
