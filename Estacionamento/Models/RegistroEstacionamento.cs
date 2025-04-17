using System.ComponentModel.DataAnnotations;
using Estacionamento.Enums;

namespace Estacionamento.Models
{
    public class RegistroEstacionamento
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string PlacaCarro { get; set; }
        [Required]
        public DateTime HorarioEntrada { get; set; }
        public DateTime? HorarioSaida { get; set; }

        public decimal ValorPagar { get; set; }

        public MetodoPagamento MetodoPagamento { get; set; }
        public bool Pago { get; set; } = false;
        
        public bool Finalizado { get; set; } = false;

    }
}


