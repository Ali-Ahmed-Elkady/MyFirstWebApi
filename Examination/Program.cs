using BLL.Mapper;

namespace Examination
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            //Add DataBaseConnection
            builder.Services.AddDb(builder.Configuration);
            //add identity service 
            builder.Services.AddIdentityDependencyInjection();
            //add jwt services 
            builder.Services.AddJWT();
            builder.Services.AddScopedServices();
            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
