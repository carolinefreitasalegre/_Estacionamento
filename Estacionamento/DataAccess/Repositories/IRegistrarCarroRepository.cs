using Estacionamento.Models;

namespace Estacionamento.DataAccess.Contratos
{
    public interface IRegistrarCarroRepository
    {
        Task<RegistroEstacionamento> RegistrarCarro(RegistroEstacionamento request);
        Task<RegistroEstacionamento> BuscarRegistro(Guid Id);
        Task<RegistroEstacionamento> FinalizarVaga(Guid Id);
        Task<List<RegistroEstacionamento>> ListarCarros();
        Task<RegistroEstacionamento> EditarRegistro(RegistroEstacionamento request);
    }
}
