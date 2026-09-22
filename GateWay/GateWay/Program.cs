using Gateway.Application.Options;
using Gateway.Application.Services.AuthService;
using Gateway.Application.Services.JwtService;
using Gateway.Domain.Constants;
using Gateway.Domain.Entities;
using Gateway.Domain.Entities.Auth;
using Gateway.Infrastructure.Persistance;
using Gateway.Infrastructure.Services.AuthService;
using Gateway.Infrastructure.Services.JwtService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GateWay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.Configure<JwtOptions>(
builder.Configuration.GetSection("Jwt"));

            builder.Services.AddDbContext<ApplicationDbContext>(
            opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("default")));
            builder.Services.AddIdentity<User, Role>()
         .AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
            });


            var jwtOptions = builder.Configuration
            .GetSection("Jwt")
            .Get<JwtOptions>()!;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {



                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
                    GetBytes(jwtOptions.Secret)),


                    ValidateIssuerSigningKey = true,

                    ValidateIssuer = false,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = false,
                    ValidAudience = jwtOptions.Audience,


                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };



            });


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerOnly", policy =>
                {
                    policy.RequireRole(RolesConstants.Customer);
                });

                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireRole(RolesConstants.Admin);
                });

                options.AddPolicy("CustomerOrAdmin", policy =>
                {
                    policy.RequireRole(RolesConstants.Customer, RolesConstants.Admin);
                });

                options.AddPolicy("AllowAnonymous", policy =>
                {
                    policy.RequireAssertion(_ => true);
                });

            });



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            Array.Empty<string>()
                        }
                    });
            });

            builder.Services.AddSingleton(TimeProvider.System);


            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services
                .AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
                .AddTransforms<UserHeadersTransformProvider>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                   "/swagger/v1/swagger.json",
                   "Gateway API");

                    options.SwaggerEndpoint(
                        "/order/swagger/v1/swagger.json",
                        "Order API");

                    options.SwaggerEndpoint(
                        "/inventory/swagger/v1/swagger.json",
                        "Inventory API");
                });
            }

            app.UseHttpsRedirection();

            // TEMPORARY DEBUG


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapReverseProxy();

            app.Run();
        }
    }
}
