namespace Examination.Dependancy_Injection
{
    public static class AddAuthentication
    {
        public static IServiceCollection AddJWT(this IServiceCollection services)
        {
            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Ali",
                    ValidAudience = "users",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456"))
                };
            });
            return services;
        }
    }
}
