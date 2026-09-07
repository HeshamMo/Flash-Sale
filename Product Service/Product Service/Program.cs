
using Microsoft.EntityFrameworkCore;
using Product_Service.MessagingQueue;
using Product_Service.MessagingQueue.Consumers;
using ProductManager.Application.Mapper;
using ProductManager.Application.Services.ProductMessageServices;
using ProductManager.Application.Services.ProductMessageServices.ProductPublisher;
using ProductManager.Application.Services.ProductService;
using ProductManager.Domain.Data;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace ProductManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Configure EF Core DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("default")));
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductPublisher, ProductPublisher>();
            builder.Services.AddScoped<IProductMessageHandler, ProductMessageHandler>();
            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMQ:HostName"],
                    UserName = configuration["RabbitMQ:UserName"],
                    Password = configuration["RabbitMQ:Password"],
                    Port = int.Parse(configuration["RabbitMQ:Port"]!),
                };

                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));


            builder.Services.AddHostedService<RabbitMQSetupService>();
            builder.Services.AddHostedService<OrderCreatedConsumer>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.MapControllers();

            app.Run();
        }
    }
}
