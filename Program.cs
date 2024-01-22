using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using OmniChat.Configurations;
using OmniChat.Hubs;
using OmniChat.Interfaces;
using OmniChat.Middlewares;
using OmniChat.Repositories;
using OmniChat.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.File(Path.Combine(builder.Environment.ContentRootPath, "Logs/log.txt"),
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
        {
            options.AddPolicy(configuration["CorsName"]!, build =>
            {
                build.WithOrigins(configuration["AllowedHosts"]!)
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(configuration["MongoConfig:ConnectionString"]));
builder.Services.AddSignalR();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],            // Replace with your issuer
        ValidAudience = configuration["Jwt:Audience"],        // Replace with your audience
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))  // Replace with your secret key
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hub/chat")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.Configure<MongoConfig>(configuration.GetSection("MongoConfig"));
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IProviderRepository, ProviderRepository>();
builder.Services.AddSingleton<IProviderService, ProviderService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<DataSeedingService>();
builder.Services.AddSingleton<IUserChannelRepository, UserChannelRepository>();
builder.Services.AddSingleton<IUserFriendRepository, UserFriendRepository>();
builder.Services.AddSingleton<UserChannelService>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddSingleton<IMessageRepository, MessageRepository>();
builder.Services.AddSingleton<PlatformService>();
builder.Services.AddSingleton<ChannelRepository>();
builder.Services.AddSingleton<ChannelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors(configuration["CorsName"]!);
app.UseExceptionHandlerMiddleware();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapHub<ChatHub>("/hub/chat");
app.Run();

Log.CloseAndFlush();