using Microsoft.EntityFrameworkCore;
using ToDo.Context;
using ToDo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddDbContext<TodoDbContext>(
    opts =>
        opts.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
    );

builder.WebHost.UseUrls("https://*:8081");

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();