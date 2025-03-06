using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace TaskManagement.Gateway.Swagger
{
    public static class SwaggerConfig
    {
        public const string Version = "v1.0.0";
        public static IServiceCollection AddSwaggerWithConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.CustomSchemaIds(type => type.FullName.Replace("+", "_"));
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Task Management",
                    Version = Version,
                    License = new OpenApiLicense() { Name = "Muhamad Wattad", Url = new Uri("https://github.com/muhamadwattad/") },
                    Contact = new OpenApiContact
                    {
                        Email = "muhamadwattad@gmail.com",
                        Name = "Muhamad Wattad",
                        Url = new Uri("https://www.linkedin.com/in/muhamadwattad/")
                    },
                    Description = "Home Assignment for Backend Developer",
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Name = HeaderNames.Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                };
                config.AddSecurityDefinition("bearerAuth", jwtSecurityScheme);
            });

            return services;
        }
    }
}
