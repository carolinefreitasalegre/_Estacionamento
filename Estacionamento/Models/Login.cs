using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
