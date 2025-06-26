namespace Examination.Dependancy_Injection
{
    public static class AddDbContext
    {
        public static IServiceCollection AddDb(this IServiceCollection services ,IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(a=>a
            .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
