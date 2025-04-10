using Estacionamento.Enums;

namespace Estacionamento.Dtos.Response
{
    public class RegistroEstacionamentoResponse
    {
        public string PlacaCarro { get; set; }
        public DateTime HorarioEntrada { get; set; }
        public DateTime? HorarioSaida { get; set; }

        public double ValorPagar { get; set; }
        public MetodoPagamento MetodoPagamento { get; set; }
        public bool Pago { get; set; } = false;
    }
}
