
using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Application.Services;
using FlashSale.OrderManager.Infrastructure.Persistance;
using FlashSale.OrderManager.Infrastructure.Persistance.Repositories;
using FlashSale.OrderManager.Infrastructure.Persistance.UnitsOfWork;
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


            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<OrderProfile>();
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IOrderService, OrderService>();

            builder.Services.AddScoped<ICurrentUser, CurrentUser>();

            builder.Services.AddSingleton(TimeProvider.System);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
