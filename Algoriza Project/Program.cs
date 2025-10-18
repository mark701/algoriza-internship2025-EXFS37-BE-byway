using Domain.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Extentions;
using Repository.Implemnetion;
using Repository.Interface;
using Service.Context;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>

options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwtAuth();
//builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddHostedService<ConnectionCleanupService>();
// To use Enums as strings without errors

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

    });



builder.Services.AddCustomJwtAuth(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials(); // IMPORTANT for SignalR

        });
});


builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();




var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Algoriza API V1");
    c.RoutePrefix = "swagger";
});


app.UseStaticFiles();




app.UseRouting();




app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapHub<ChatHub>("/chathub"); // Hub must be inside UseEndpoints
//});

app.Run();
