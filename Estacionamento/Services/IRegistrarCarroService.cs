using Estacionamento.Dtos.Request;
using Estacionamento.Models;

namespace Estacionamento.Services
{
    public interface IRegistrarCarroService
    {
        Task<RegistroEstacionamento> RegistrarCarro(RegistroEstacionamentoRequest request);
        Task<RegistroEstacionamento> EfetuarPagamento(Guid Id, RegistroEstacionametoEdicaoRequest request);
        Task<RegistroEstacionamento> EditarRegistro(RegistroEstacionametoEdicaoRequest request);
        Task<List<RegistroEstacionamento>> ListarCarros();
        Task<RegistroEstacionamento> BuscarRegistro(Guid Id);
    }
}

