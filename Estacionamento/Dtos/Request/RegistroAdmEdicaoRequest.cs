using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Dtos.Request
{
    public class RegistroAdmEdicaoRequest
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }

        public bool UsuarioValido { get; set; } = true;
    }
}
