using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.Data;
using ProdLib;
using UserApiService.Models;
using AutoMapper;
using UserApiService.Data;
//using UserApiService.MessagePublishers;
using ProdLib.SearchProducts;
using ValidationLib;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using dotenv.net;
using Serilog;
using UserApiService.Services;
using UserApiService.Attributes;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load(options: new DotEnvOptions(probeForEnv: true, probeLevelsToSearch: 2));

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(Log.Logger);
    });


builder.Services.AddAuthorization();

builder.Services.AddHttpClient();

builder.Services.AddScoped<JwtAuthorizeAttribute>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});
bool IsRunningInContainer()
{
    return bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;
}


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("ServiceConnectionLocal");
    if (IsRunningInContainer() == true) connectionString = builder.Configuration.GetConnectionString("ServiceConnectionContainer");
    options.UseSqlServer(connectionString);
});



builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<AzureAuthenticationService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//* Configuring Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = null;
})
// * Validator used to allow storing the Email as username
.AddRoles<IdentityRole>()
.AddUserValidator<UserValidator>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
   {
       options.AddPolicy("AdminPolicy", policy =>
           policy.RequireRole("Admin"));
   });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//* Swagger authorization configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });

    // Add security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddTransient<TokenService>();


//* Configuring Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //? Set default scheme for logging into the application itself.
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) //? Send default cookie
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("ECT_JWT_ISSUER"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ECT_JWT_KEY")))
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Add roles to db if they don't exist
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


    foreach (var role in new[] { "Admin", "PromoUser" })
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //* Configuring Swagger UI
    app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

    c.EnableDeepLinking();
    c.DisplayRequestDuration();
    c.DefaultModelsExpandDepth(-1);
    c.ShowExtensions();
    c.DefaultModelExpandDepth(2);
    c.DefaultModelRendering(ModelRendering.Model);
    c.DisplayOperationId();
    c.DocumentTitle = "UserService";
});
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();

app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //* Configuring Swagger UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // Other Swagger configurations...
    });
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
