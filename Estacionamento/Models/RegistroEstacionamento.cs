using Estacionamento.Enums;

namespace Estacionamento.Models
{
    public class RegistroEstacionamento
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public string PlacaCarro { get; set; }
        public DateTime HorarioEntrada { get; set; }
        public DateTime? HorarioSaida { get; set; }

        public decimal ValorPagar { get; set; }

        public MetodoPagamento MetodoPagamento { get; set; }
        public bool Pago { get; set; } = false;
        
        public bool Finalizado { get; set; } = false;

    }
}


