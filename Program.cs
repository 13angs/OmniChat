using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using OmniChat.Controllers;
using OmniChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;
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
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(configuration["MongoSetting:ConnectionString"]));
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
});

var app = builder.Build();

// Inside your user creation logic
string password = "user_password"; // Replace with the actual user's password
byte[] passwordHash, passwordSalt;

PasswordHasher.CreatePasswordHash(password, out passwordHash, out passwordSalt);

using (var scope = app.Services.CreateAsyncScope())
{
    var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var database = mongoClient.GetDatabase("omni_db");
    var usersCollection = database.GetCollection<User>("users");
    var users = usersCollection.Find(_ => true).ToEnumerable();

    if (!users.Any())
    {
        logger.LogInformation("No user exist!\nAdding new users...");
        users = UserGeneration.GenerateUserList(passwordHash, passwordSalt);

        await usersCollection.InsertManyAsync(users);
        // await usersCollection.InsertManyAsync(new List<User>{
        //     new User{
        //         Id="c13cf23b-59c8-490d-8e8b-93c1988e203b",
        //         Name="user 1",
        //         Username="user1",
        //         Avatar="https://cdn.dribbble.com/users/3841177/screenshots/11950347/cartoon-avatar_2020__8_circle.png",
        //         PasswordHash = passwordHash,
        //         PasswordSalt = passwordSalt,
        //     },
        //     new User{
        //         Id="29e7066b-0c42-48de-a835-1a103fd1dade",
        //         Name="user 2",
        //         Username="user2",
        //         Avatar="https://www.gamer-hub.io/static/img/team/sam.png",
        //         PasswordHash = passwordHash,
        //         PasswordSalt = passwordSalt,
        //     },
        // });
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors(configuration["CorsName"]!);

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapHub<ChatHub>("/hub/chat");
app.Run();

public class PasswordHasher
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}

public class UserGeneration
{
    public static List<User> GenerateUserList(byte[] passwordHash, byte[] passwordSalt)
    {
        var users = new List<User>();

        Random random = new Random();

        for (int i = 1; i <= 50; i++)
        {
            string randomName = GenerateRandomString(random, 5); // You can specify the length you desire
            string randomUsername = $"user-{randomName}"; // You can specify the length you desire

            users.Add(new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = randomName,
                Username = randomUsername,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            });
        }

        return users;
    }

    private static string GenerateRandomString(Random random, int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}