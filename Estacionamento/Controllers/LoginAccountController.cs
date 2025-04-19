using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Estacionamento.DataAccess.ContextApi;
using Estacionamento.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Estacionamento.Controllers
{

    public class LoginAccountController : Controller
    {

        private readonly AppDbContext _context;

        public LoginAccountController(AppDbContext context)
        {
            _context = context;
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
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

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




        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Login");
        }
    }
}
