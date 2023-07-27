using Domain.Helpers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Ioc;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionTarefa");

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                     policy =>
                     {
                         policy.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                     });
});

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Point", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
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


builder.Services.AddInfraStructure(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));

// JWT
var key = Encoding.ASCII.GetBytes(CodigoCripto.Cripto());

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // Configura a valida��o da expira��o do token
            ClockSkew = TimeSpan.Zero // Define a margem de tempo para considerar o token como inv�lido
        };
    });


builder.Services.AddAuthorization(option =>
{
    option.AddPolicy(name: "Admin", policy => policy.RequireRole("Lojista"));
    option.AddPolicy(name: "Admin", policy => policy.RequireRole("Atendente"));
    option.AddPolicy(name: "Admin", policy => policy.RequireRole("Entregador"));
    option.AddPolicy(name: "Admin", policy => policy.RequireRole("Administrador"));
});
// JWT

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Entrega v1"));
//}


app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();   // JWT
app.UseAuthorization();   // JWT
app.MapControllers();
app.Run();
