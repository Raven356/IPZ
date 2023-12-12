using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogMVC.BLL;
using BlogMVC;
using BlogMVC.DAL.Context;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var dbMongo = Environment.GetEnvironmentVariable("DB_MONGO");
//var connectionString = builder.Configuration.GetConnectionString("BlogMVCContext")
//    ?? throw new InvalidOperationException("Connection string 'BlogMVCContext' not found.");
var connectionString = $"Data source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";
//var connectionString = $"Data source=host.docker.internal;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password={dbPassword}";
var mongoConnection = $"{dbMongo}://{dbMongo}:27017";
DependencyResolver.Configure(builder.Services, connectionString, mongoConnection);

builder.Services.AddAutoMapper(typeof(WebMappingProfile).Assembly);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
});

// Inside the Configure method


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Account/Login");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name V1");
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<BlogMVCContext>(); // Replace YourDbContext with your actual DbContext
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Applying migrations and seeding the database...");

        // Apply pending migrations
        dbContext.Database.Migrate();

        // Additional seeding logic if needed
        // e.g., dbContext.EnsureSeedData();

        logger.LogInformation("Database migration and seeding completed successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BlogPosts}/{action=Index}/{id?}");

app.Run();
