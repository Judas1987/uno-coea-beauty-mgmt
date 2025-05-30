using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal;
using SalonManagement.Services.Interfaces;
using SalonManagement.Services.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<SalonDbContext>(options => 
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services
            .AddScoped<IAppointmentsService, AppointmentsService>()
            .AddScoped<ICustomersService, CustomersService>()
            .AddScoped<IPackagesService, PackagesService>()
            .AddScoped<IServiceCategoriesService, ServiceCategoriesService>()
            .AddScoped<IServicePackagesService, ServicePackagesService>()
            .AddScoped<IServicesService, ServicesService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();


        app.Run();
    }
}


