using Estacionamento.Models;

namespace Estacionamento.Services
{
    public interface IPagamento
    {
        Task<RegistroEstacionamento> ProcessarPagamento(RegistroEstacionamento registro);
    }
}
