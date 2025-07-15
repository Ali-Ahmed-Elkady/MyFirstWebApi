using Microsoft.OpenApi.Models;

namespace Examination.Dependancy_Injection
{
    public static class SwaggerOptions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services) 
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Examination API",
                    Description = "Examination API for managing activities, customers, and tariffs",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme ="Bearer",
                    BearerFormat ="Jwt",
                    In = ParameterLocation.Header,
                    Description= "Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345asasas",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
