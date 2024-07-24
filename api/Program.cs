using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using api.Data;
using api.Services;
using api.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Meet Management System API",
        Version = "v1"
    });

    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        var controller = apiDesc.ActionDescriptor.RouteValues["controller"];
        return controller != "Home";
    });

    // Add JWT authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter JWT token into field"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// CORS politikası ekleme
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// JWT Authentication yapılandırması
var jwtSecretKey = "YourStrong@JwtSecretKey1234567890"; 
var key = Encoding.ASCII.GetBytes(jwtSecretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5229", 
            ValidAudience = "http://localhost:5229", 
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meet Management System API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Token doğrulama ve yönlendirme
// app.Use(async (context, next) =>
// {
//     var path = context.Request.Path.Value;

//     // Log path
//     System.Console.WriteLine($"Requested Path: {path}");

//     // Giriş sayfasına yönlendirme kontrolü
//     if (path == "/anasayfa" || path == "/toplantı_listesi" || path == "/toplantı_ekle")
//     {
//         var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

//         // Token'u konsola yazdır
//         System.Console.WriteLine($"Token: '{token}'");

//         if (string.IsNullOrEmpty(token))
//         {
//             context.Response.Redirect("/");
//             return;
//         }

//         try
//         {
//             var tokenHandler = new JwtSecurityTokenHandler();
//             var validationParameters = new TokenValidationParameters
//             {
//                 ValidateIssuer = true,
//                 ValidateAudience = true,
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("YourStrong@JwtSecretKey1234567890")),
//                 ValidateLifetime = true,
//                 ValidIssuer = "http://localhost:5229",
//                 ValidAudience = "http://localhost:5229"
//             };

//             tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

//             if (validatedToken == null)
//             {
//                 context.Response.Redirect("/");
//                 return;
//             }
//         }
//         catch (Exception ex)
//         {
//             System.Console.WriteLine($"Token doğrulama hatası: {ex.Message}");
//             context.Response.Redirect("/");
//             return;
//         }
//     }

//     await next();
// });

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
