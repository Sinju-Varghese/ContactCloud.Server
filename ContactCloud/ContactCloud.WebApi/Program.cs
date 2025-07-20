using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ContactCloud.Entity.Data;  
using ContactCloud.Entity.Model;
using ContactCloud.Services.Repository;
using ContactCloud.Services.Repositories;
using FluentValidation;
using ContactCloud.Services.Dto;
using ContactCloud.Services.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ContactCloud.Services.Dto.Contact;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Name = "Bearer", // or API Key name
        Scheme = "bearer",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Description = "JWT Auth: Bearer {token}" // or API key description
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="bearerAuth"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<ContactCloudDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ContactCloud.Entity")
    ));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ContactCloudDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
builder.Services.AddScoped<IValidator<RegisterRequestDto>, RegisterUserValidator>();
builder.Services.AddScoped<IValidator<SignInDto>, SignInValidator>();
builder.Services.AddScoped<IValidator<ContactCreateDto>, ContactCreateDtoValidator>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(O =>
{
    O.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        NameClaimType = "sub"
    };
});
//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
