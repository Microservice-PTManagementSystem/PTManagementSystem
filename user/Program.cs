using FluentValidation;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Application.Mappings;
using PTManagementSystem.Application.Services;
using PTManagementSystem.Application.Validators;
using PTManagementSystem.Infrastructure.ExternalServices;
using PTManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using PTManagementSystem.Infrastructure.Data;
using PTManagementSystem.Application.UseCases.Commands;
using PTManagementSystem.Application.UseCases.Queries;
using PTManagementSystem.Infrastructure.Config;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile)); // MappingProfile senin Application.Mappings içindeki sınıf

// FluentValidation 
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PaymentInfoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddressValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TrainerProfileValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserProfileValidator>();

// UseCases ve Services
builder.Services.AddScoped<RegisterUser>();
builder.Services.AddScoped<LoginUser>();
builder.Services.AddScoped<DeleteUser>();
builder.Services.AddScoped<UpdatePaymentInfo>();
builder.Services.AddScoped<UpdateTrainerProfile>();
builder.Services.AddScoped<UpdateUserProfile>();

builder.Services.AddScoped<GetPaymentInfo>();
builder.Services.AddScoped<GetTrainerProfile>();
builder.Services.AddScoped<GetUserProfile>();
builder.Services.AddScoped<GetUser>();


builder.Services.AddScoped<IKeycloakService, KeycloakService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IMessageBroker, RabbitMQService>();
builder.Services.AddScoped<IUserValidationService, UserValidationService>();
builder.Services.Configure<KeycloakSettings>(
    builder.Configuration.GetSection("Keycloak"));

builder.Services.AddSingleton<UserDbContext>();
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Env.Load();

// // Environment değişkenleri
// builder.Configuration["Keycloak:Realm"] = Environment.GetEnvironmentVariable("KEYCLOAK_REALM");
// builder.Configuration["Keycloak:ClientId"] = Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_ID");
// builder.Configuration["Keycloak:ClientSecret"] = Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_SECRET");
// builder.Configuration["Keycloak:AdminUsername"] = Environment.GetEnvironmentVariable("KEYCLOAK_ADMIN");
// builder.Configuration["Keycloak:AdminPassword"] = Environment.GetEnvironmentVariable("KEYCLOAK_ADMIN_PASSWORD");

// builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddHttpClient<IKeycloakService, KeycloakService>();

builder.Configuration.AddEnvironmentVariables();
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<UserDbContext>(options =>
// {
//     options.UseMySql(
//         builder.Configuration.GetConnectionString("DefaultConnection"),
//         new MySqlServerVersion(new Version(8, 0, 0))
//     );
// });

var corsPolicyName = "AllowFrontend";

// CORS ayarları
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()|| true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthorization();

app.MapControllers();

app.Run();
