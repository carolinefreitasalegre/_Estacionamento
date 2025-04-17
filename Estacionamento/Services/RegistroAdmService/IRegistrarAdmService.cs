using Estacionamento.Dtos.Request;
using Estacionamento.Dtos.Response;
using Estacionamento.Models;

namespace Estacionamento.Services.RegistroAdmService
{
    public interface IRegistrarAdmService
    {
        Task<Administrador> CadastrarAdm(RegistroAdmRequest request);
        Task<Administrador> EditarAdm(Guid Id, RegistroAdmEdicaoRequest request);
        Task<Administrador> BuscarAdm(Guid Id);
        Task<List<Administrador>> ListarAdm();
        Task<Administrador> DesabilitarAdm(Guid Id);
    }
}
