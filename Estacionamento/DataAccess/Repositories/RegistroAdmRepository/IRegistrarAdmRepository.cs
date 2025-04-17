using Estacionamento.Models;

namespace Estacionamento.DataAccess.Repositories.RegistroAdmRepository
{
    public interface IRegistrarAdmRepository
    {
        Task<Administrador> CadastrarAdm(Administrador adm);
        Task<Administrador> EditarAdm(Guid Id, Administrador adm);
        Task<Administrador> BuscarAdm(Guid Id);
        Task<List<Administrador>> ListarAdm();
        Task<Administrador> DesabilitarAdm(Guid Id);
    }
}
