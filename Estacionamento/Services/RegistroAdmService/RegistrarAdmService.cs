using Estacionamento.DataAccess.Repositories.RegistroAdmRepository;
using Estacionamento.Dtos.Request;
using Estacionamento.Dtos.Response;
using Estacionamento.Models;

namespace Estacionamento.Services.RegistroAdmService
{
    public class RegistrarAdmService : IRegistrarAdmService
    {

        private readonly IRegistrarAdmRepository _repository;

        public RegistrarAdmService(IRegistrarAdmRepository repository)
        {
            _repository = repository;
        }

        public async Task<Administrador> BuscarAdm(Guid Id)
        {
            var adm = await _repository.BuscarAdm(Id) ?? throw new Exception("Administrador não encontrado.");

            return adm;
        }

        public async Task<Administrador> CadastrarAdm(RegistroAdmRequest request)
        {

            var adm = new Administrador(request.Nome, request.Email, request.Senha)
            {
                Nome = request.Nome.ToLower(),
                Email = request.Email.ToLower(),
                Senha = request.Senha,

                UsuarioValido = request.UsuarioValido,
            };

            await _repository.CadastrarAdm(adm);

            return adm;
        }

        public async Task<Administrador> DesabilitarAdm(Guid Id)
        {
            var adm = await _repository.BuscarAdm(Id) ?? throw new Exception("Administrador não encontrado.");

            adm.UsuarioValido = false;

            return await _repository.DesabilitarAdm(Id);
        }

        public async Task<Administrador> EditarAdm(Guid Id, RegistroAdmEdicaoRequest request)
        {

            var adm = await _repository.BuscarAdm(Id) ?? throw new Exception("Administrador não encontrado.");

            adm.Nome = request.Nome.ToLower();
            adm.Email = request.Email.ToLower();
            adm.Senha = request.Senha;
            adm.UsuarioValido = request.UsuarioValido;

            return await _repository.EditarAdm(Id, adm);

        }

        public async Task<List<Administrador>> ListarAdm()
        {
            return await _repository.ListarAdm();
        }
    }
}
