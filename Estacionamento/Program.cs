using System.Security.Cryptography;
using System.Text;
using Estacionamento.DataAccess.ContextApi;
using Estacionamento.DataAccess.Contratos;
using Estacionamento.DataAccess.Repositories;
using Estacionamento.Dtos.Request;
using Estacionamento.Services;
using Estacionamento.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);






builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("88cae4db-56cf-4ec0-be44-376fea86ca47")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };

    // Permite ler o token tanto do header quanto do cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["access_token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddScoped<EstacionamentoCalculoService>();
builder.Services.AddScoped<IRegistrarCarroRepository, RegistrarCarroRepository>();
builder.Services.AddScoped<IRegistrarCarroService, RegistrarCarroService>();
builder.Services.AddTransient<IValidator<RegistroEstacionamentoRequest>, EstacionamentoRegistroValidator>();
builder.Services.AddTransient<IValidator<RegistroEstacionametoEdicaoRequest>, EdicaoEstacionamentoRegistroValidation>();


// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});




var app = builder.Build();



app.Use(async (context, next) =>
{
    // Log all headers for debugging
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"{header.Key}: {header.Value}");
    }

    await next();
});




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Estacionamento/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LoginAccount}/{action=Login}/{id?}");

app.Run();
