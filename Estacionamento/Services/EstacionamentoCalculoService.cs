namespace Estacionamento.Services
{
    public class EstacionamentoCalculoService
    {

        private decimal PrecoMeiaHora = 15.00m;

        public TimeSpan CalcularTempoEstacionado(DateTime entrada, DateTime? saida)
        {
            if (saida == null)
                saida = DateTime.Now;

            return saida.Value - entrada;
        }

        public decimal CalcularValorPagar(DateTime entrada, DateTime? saida)
        {
            var tempo = CalcularTempoEstacionado(entrada, saida);

            int total = (int)Math.Ceiling(tempo.TotalMinutes / 30.0);

            return total * PrecoMeiaHora;
        }
    }
}
