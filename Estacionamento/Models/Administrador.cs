using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Models
{
    public class Administrador
    {
        public Administrador(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }

        [Key]
        public Guid Id { get; set; }
        [Required, StringLength(30)]

        public string Nome { get; set; }
        [Required, StringLength(30)]
        [EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(30),]
        public string Senha { get; set; }

        public bool UsuarioValido { get; set; } = true;
    }
}
