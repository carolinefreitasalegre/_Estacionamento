using Estacionamento.Models;

namespace Estacionamento.DataAccess.Contratos
{
    public interface IRegistrarCarroRepository
    {
        Task<RegistroEstacionamento> RegistrarCarro(RegistroEstacionamento request);
        Task<RegistroEstacionamento> BuscarRegistro(Guid Id);
        Task<RegistroEstacionamento> EfetuarPagamento(Guid Id, RegistroEstacionamento request);
        Task<List<RegistroEstacionamento>> ListarCarros();
        Task<RegistroEstacionamento> EditarRegistro(RegistroEstacionamento request);
    }
}
