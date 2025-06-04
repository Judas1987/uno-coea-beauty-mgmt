using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SalonManagement.Api.Models.Appointments;
using SalonManagement.Api.Validation.Appointments;
using SalonManagement.Dal;
using SalonManagement.Services.Interfaces;
using SalonManagement.Services.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Register FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:5027", "https://localhost:7121")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        // Configure Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Beauty Salon Management API",
                Version = "v1",
                Description = "API for managing beauty salon appointments, services, and customers",
                Contact = new OpenApiContact
                {
                    Name = "Development Team",
                    Email = "dev@example.com"
                }
            });
        });

        // Add AutoMapper
        builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ServicesService).Assembly);

        // Add FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateAppointmentRequestValidator>();

        builder.Services.AddDbContext<SalonDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services
            .AddScoped<IAppointmentsService, AppointmentsService>()
            .AddScoped<ICustomersService, CustomersService>()
            .AddScoped<IPackagesService, PackagesService>()
            .AddScoped<IServiceCategoriesService, ServiceCategoriesService>()
            .AddScoped<IServicePackagesService, ServicePackagesService>()
            .AddScoped<IServicesService, ServicesService>()
            .AddScoped<ISalonDbContext, SalonDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Beauty Salon Management API V1");
                c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
            });
        }

        // Enable CORS - must be before routing and endpoints
        app.UseCors();

        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}


