using BLL.Services.ActivityTypeService;
using BLL.Services.CustomersService;
using BLL.Services.TariffService;
using BLL.Services.Users;
using DAL.Entities;
using DAL.Repo.Abstraction;
using DAL.Repo.Implementation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Examination.Dependancy_Injection
{
    public static class ScopedServices
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepo<>),typeof(Repo<>));
            services.AddScoped<IUser, User>();
            services.AddScoped<ICustomer, Customer>();
            services.AddScoped<IActivityTypeService, ActivityTypeService>();
            services.AddScoped<ITariffService, TariffService>();
            return services;
        }
    }
}
