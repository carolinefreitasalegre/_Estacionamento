using System.ComponentModel.DataAnnotations;

namespace Estacionamento.Dtos.Request
{
    public class RegistroEstacionametoEdicaoRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Placa não pode estar em branco.")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "A placa deve ter 7 caracteres.")]
        public string PlacaCarro { get; set; }
        public DateTime HorarioEntrada { get; set; }
    }
}
