using FlashSale.InventoryManager.API.Services;
using FlashSale.InventoryManager.Application.Interfaces;
using FlashSale.InventoryManager.Application.Mapping;
using FlashSale.InventoryManager.Infrastructure;
using FlashSale.InventoryManager.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
namespace FlashSale.InventoryManager.API
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
                cfg.AddProfile<ProductProfile>();
            });

            builder.Services.AddHttpContextAccessor();



            builder.Services.AddScoped<ICurrentUser, CurrentUser>();

            builder.Services.AddSingleton(TimeProvider.System);

            builder.Services.AddInfrastructure(builder.Configuration);

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
