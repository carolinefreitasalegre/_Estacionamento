using System.Text;
using Estacionamento.DataAccess.ContextApi;
using Estacionamento.DataAccess.Repositories.RegistroAdmRepository;
using Estacionamento.DataAccess.Repositories.RegistroCarroRepository;
using Estacionamento.Dtos.Request;
using Estacionamento.Services.PagamentoService;
using Estacionamento.Services.RegistroAdmService;
using Estacionamento.Services.RegistroCarroService;
using Estacionamento.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{

    var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
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



builder.Services.AddTransient<IValidator<RegistroEstacionamentoRequest>, EstacionamentoRegistroValidator>();
builder.Services.AddTransient<IValidator<RegistroEstacionametoEdicaoRequest>, EdicaoEstacionamentoRegistroValidation>();
builder.Services.AddTransient<IValidator<RegistroAdmRequest>, EstacionamentoRegistroAdminValidator>();
builder.Services.AddTransient<IValidator<RegistroAdmEdicaoRequest>, EdicaoAdminValidator>();


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});




var app = builder.Build();




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
