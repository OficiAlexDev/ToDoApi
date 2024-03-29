using Microsoft.EntityFrameworkCore;
using ToDo.Context;
using ToDo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add autorization services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    bearerOptions =>
    {
        bearerOptions.RequireHttpsMetadata = false;
        bearerOptions.SaveToken = true;
        bearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(JWTSecret.Key(builder.Configuration)),
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    }
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Define Swagger authentication options
builder.Services.AddSwaggerGen(setup =>
{
    OpenApiSecurityScheme jwtBearer = new()
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Description = "Put only JWT token!",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    };
    setup.AddSecurityDefinition("Bearer", jwtBearer);
    setup.AddSecurityRequirement(new()
    {
        {
            //new OpenApiSecurityScheme
            new()
            {
                //new OpenApiReference
                Reference = new()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id =  JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

//Setting Db Context to use Postgress by Npgsql
builder.Services.AddDbContext<TodoDbContext>(
    opts =>
        opts.UseNpgsql(builder.Configuration.GetConnectionString("DbHost"))
);

//Setting Redis
builder.Services.AddStackExchangeRedisCache(opts =>
    {
        opts.InstanceName = "cacheToDo";
        opts.Configuration = builder.Configuration.GetConnectionString("CacheHost");
    });

// Add dependency injept for JWT Service class
builder.Services.AddScoped<JWTServices>();
// Add dependency injept for Redis class
builder.Services.AddScoped<Redis>();

builder.WebHost.UseUrls("https://*:8081");

//Add cors to test API
string corsKey = "TestLocalDevelopment";
builder.Services.AddCors(opts =>
{
    opts.AddPolicy(name: corsKey, policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:4200", "http://localhost:4201", "http://127.0.0.1:5500");
    });
});


var app = builder.Build();

//Add cors to test API
app.UseCors(corsKey);

//Database migrate at runtime
using (IServiceScope scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<TodoDbContext>().Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Define app to use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();