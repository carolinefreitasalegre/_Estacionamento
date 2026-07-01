using System.Text;
using Estacionamento.DataAccess.ContextApi;
using Estacionamento.DataAccess.Repositories.RegistroAdmRepository;
using Estacionamento.DataAccess.Repositories.RegistroCarroRepository;
using Estacionamento.Dtos.Request;
using Estacionamento.Services.GerarRelatorio;
using Estacionamento.Services.PagamentoService;
using Estacionamento.Services.RegistroAdmService;
using Estacionamento.Services.RegistroCarroService;
using Estacionamento.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var envFile = $".env.{environment.ToLower()}";

if (File.Exists(envFile))
{
    Env.Load(envFile);
}
else
{
    Env.Load();
    Console.WriteLine($"Ambiente: {environment}");
    Console.WriteLine($"Procurando arquivo: {envFile}");
    Console.WriteLine($"Arquivo existe? {File.Exists(envFile)}");
    Console.WriteLine($"Diretório atual: {Directory.GetCurrentDirectory()}");
}

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["JWT_SECRET_KEY"];
var connectionString = builder.Configuration["DB_CONNECTION_STRING"];

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };

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
builder.Services.AddScoped<IRegistrarAdmRepository, RegistrarAdmRepository>();
builder.Services.AddScoped<IRegistrarAdmService, RegistrarAdmService>();
builder.Services.AddScoped<IGerarRelatorioService, GerarRelatorioService>();

builder.Services.AddTransient<IValidator<RegistroEstacionamentoRequest>, EstacionamentoRegistroValidator>();
builder.Services.AddTransient<IValidator<RegistroEstacionametoEdicaoRequest>, EdicaoEstacionamentoRegistroValidation>();
builder.Services.AddTransient<IValidator<RegistroAdmRequest>, EstacionamentoRegistroAdminValidator>();
builder.Services.AddTransient<IValidator<RegistroAdmEdicaoRequest>, EdicaoAdminValidator>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Estacionamento/Error");
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