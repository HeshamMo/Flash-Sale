
using FlashSale.OrderManager.API.Middlewares;
using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Application.Options;
using FlashSale.OrderManager.Application.Services;
using FlashSale.OrderManager.Infrastructure;
using FlashSale.OrderManager.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using OrderManager.Application.Mapping;
namespace FlashSale.OrderManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));


            builder.Services.Configure<OutboxOptions>(
    builder.Configuration.GetSection("Outbox"));


            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<OrderProfile>();
            });




            builder.Services.AddHttpContextAccessor();



            builder.Services.AddScoped<ICurrentUser, CurrentUser>();

            builder.Services.AddSingleton(TimeProvider.System);


            builder.Services.AddInfrastructure(configuration: builder.Configuration);




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseMiddleware<AddStaticUserDevelopmentMiddleware>();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
