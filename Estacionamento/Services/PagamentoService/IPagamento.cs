using Estacionamento.Models;

namespace Estacionamento.Services.PagamentoService
{
    public interface IPagamento
    {
        Task<RegistroEstacionamento> ProcessarPagamento(RegistroEstacionamento registro);
    }
}
