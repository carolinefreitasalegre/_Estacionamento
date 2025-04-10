namespace Estacionamento.Models
{
    public class TabelaValor
    {
        public Guid Id { get; set; }
        public decimal PrecoPorMeiaHora { get; set; } = 15.00m;
        public int TempoToleranciaMinutos { get; set; } = 5;
    }
}
