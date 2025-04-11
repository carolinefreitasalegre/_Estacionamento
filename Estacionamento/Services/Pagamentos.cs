using Estacionamento.Models;

namespace Estacionamento.Services
{
    public class Pagamentos
    {
        public class PagamentoPix : IPagamento
        {
            public Task<RegistroEstacionamento> ProcessarPagamento(RegistroEstacionamento registro)
            {
                registro.Pago = true;
                return Task.FromResult(registro);
            }
        }

        public class PagamentoDebito : IPagamento
        {
            public Task<RegistroEstacionamento> ProcessarPagamento(RegistroEstacionamento registro)
            {
                registro.Pago = true;
                return Task.FromResult(registro);
            }
        }

        public class PagamentoCredito : IPagamento
        {
            public Task<RegistroEstacionamento> ProcessarPagamento(RegistroEstacionamento registro)
            {
                registro.Pago = true;
                return Task.FromResult(registro);
            }
        }
    }
}
