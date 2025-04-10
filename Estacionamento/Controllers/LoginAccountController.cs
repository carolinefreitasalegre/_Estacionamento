using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Estacionamento.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Estacionamento.Controllers
{
    public class LoginAccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(Login login)
        {
            if (login.Email == "admin" && login.Senha == "admin")
            {
                var token = GerarToken();

                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, 
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.Now.AddMinutes(30)
                });

                return RedirectToAction("Index", "Estacionamento");
            }

            TempData["Mensagem"] = "Credenciais inválidas";
            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Login");
        }
        private string GerarToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("88cae4db-56cf-4ec0-be44-376fea86ca47");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, "admin"),
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
    }
}
