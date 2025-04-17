using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Dtos.Request
{
    public class RegistroAdmRequest
    {

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; }


        public bool UsuarioValido { get; set; } = true;
    }
}
