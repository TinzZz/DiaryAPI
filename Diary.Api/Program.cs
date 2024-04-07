using Diary.DAL.DependencyInjection;
using Diary.Application.DependencyInjection;
using Serilog;
using Diary.Domain.Settings;
using Diary.Consumer.DependencyInjection;
using Diary.Producer.DependensyInjection;

namespace Diary.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
            builder.Services.AddControllers();
            builder.Services.AddAuthenticationAndAuthorization(builder);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwagger();
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
            builder.Services.AddDataAccessLayer(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddProducer();
            builder.Services.AddConsumer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diary Swagger v1.0");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Diary Swagger v2.0");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
