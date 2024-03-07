using Domain.Services;
using Logic.Command;
using Logic.Command.Factory;
using Logic.Handler;
using Logic.Handler.Abstraction;
using Logic.Provider.Command;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Database;
using Server.Options;
using Server.Repositories;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.GetSection(AliceSetting.Position).Get<AliceSetting>();
var jwtConfig = builder.Configuration.GetSection(JWT.Position).Get<JWT>();

// Services
{
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.Decorate<IUserService, CachedUserService>();
    builder.Services.AddScoped<IJokeService, JokeService>();
    builder.Services.AddScoped<ICommandExecutor, CommandExecutor>();
    builder.Services.AddScoped<ICommandFactory, CommandFactory>();
    builder.Services.AddSingleton<ICommandDataProvider, CommandDataProvider>();
    builder.Services.Decorate<ICommandDataProvider, CachedCommandDataProvider>();
}

//Repository
{
    builder.Services.AddScoped<UserRepository>();
    builder.Services.AddScoped<JokeRepository>();
}

builder.Services.AddDbContextFactory<AppDbContext>(ConfigurePostgresConnection);
builder.Services.AddDbContext<AppDbContext>(ConfigurePostgresConnection);
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.ToString());
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = jwtConfig.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

void ConfigurePostgresConnection(DbContextOptionsBuilder options)
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlContext"));
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
app.Run();