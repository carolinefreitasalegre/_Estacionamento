using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Estacionamento.DataAccess.ContextApi;
using Estacionamento.Dtos.Request;
using Estacionamento.Models;
using Estacionamento.Services.RegistroAdmService;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Estacionamento.Controllers
{

    public class LoginAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRegistrarAdmService _service;
        private readonly IValidator<RegistroAdmRequest> _validator;
        
        public LoginAccountController(AppDbContext context, IConfiguration configuration, IRegistrarAdmService service, IValidator<RegistroAdmRequest> validator)
        {
            _context = context;
            _configuration = configuration;
            _service = service;
            _validator = validator;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var usuario = await _context.Administradores
                .FirstOrDefaultAsync(x => x.Email == login.Email && x.Senha == login.Senha);

            if (usuario != null && usuario.UsuarioValido == true)
            {
                var token = GerarToken(usuario);
                Console.WriteLine(token);
                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.Now.AddMinutes(30)
                });

                return RedirectToAction("Index", "Estacionamento");
            }

            TempData["Mensagem"] = "Credenciais inválidas ou usuário sem permissão de entrada!";
            return View();
        }
        
        private string GerarToken(Administrador usuario)
        {
            var jwtKey = _configuration["Jwt:SecretKey"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("A chave secreta do JWT não foi configurada nas variáveis de ambiente do servidor.");
            }
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, "Administrator")
        }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("/criar-conta")]
        public IActionResult CriarConta()
        {
            return View();
        }

        
        [HttpPost("/criar-conta")]
        public async Task<IActionResult> CriarConta(RegistroAdmRequest request)
        {
            try
            {
                var validator = await _validator.ValidateAsync(request);

                if (!validator.IsValid)
                {
                    foreach (var erro in validator.Errors)
                    {
                        ModelState.AddModelError(erro.PropertyName, erro.ErrorMessage);
                    }

                    return View(request);
                }

                var adm = await _service.CadastrarAdm(request);
                TempData["Mensagem"] = "Usuário cadastrado com sucesso!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar cadastrar o administrador.", ex);
            }
        }



        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Login");
        }
    }
}
