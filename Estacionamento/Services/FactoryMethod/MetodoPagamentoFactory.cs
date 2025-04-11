using Estacionamento.Enums;
using static Estacionamento.Services.Pagamentos;

namespace Estacionamento.Services.FactoryMethod
{
    public class MetodoPagamentoFactory
    {
        public static IPagamento GerarMetodoPagamento(MetodoPagamento metodo)
        {
            return metodo switch
            {
                MetodoPagamento.Pix => new PagamentoPix(),
                MetodoPagamento.Credito => new PagamentoCredito(),
                MetodoPagamento.Debito => new PagamentoDebito(),
                _ => throw new NotImplementedException("Método de pagamento não implementado")

            };
        }
    }
}
