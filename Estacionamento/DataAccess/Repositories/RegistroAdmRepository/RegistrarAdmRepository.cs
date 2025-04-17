using Estacionamento.DataAccess.ContextApi;
using Estacionamento.Models;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.DataAccess.Repositories.RegistroAdmRepository
{
    public class RegistrarAdmRepository : IRegistrarAdmRepository
    {

        private readonly AppDbContext _context;

        public RegistrarAdmRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Administrador> BuscarAdm(Guid Id)
        {
            var adm = await _context.Administradores.FirstOrDefaultAsync(a => a.Id == Id) ?? throw new Exception("Administrador não encontrado.");

            return adm;
        }

        public async Task<Administrador> EditarAdm(Guid Id, Administrador adm)
        {
            var admin = await _context.Administradores.FirstOrDefaultAsync(a => a.Id == Id) ?? throw new Exception("Administrador não encontrado.");

            _context.Update(admin);
            await _context.SaveChangesAsync();

            return adm;
        }

        public async Task<Administrador> CadastrarAdm(Administrador adm)
        {
            _context.Add(adm);
            await _context.SaveChangesAsync();  

            return adm;
        }

        public async Task<Administrador> DesabilitarAdm(Guid Id)
        {
            var admin = await _context.Administradores.FirstOrDefaultAsync(a => a.Id == Id) ?? throw new Exception("Administrador não encontrado.");

            _context.Update(admin);
            await _context.SaveChangesAsync();

            return admin;
        }

       

        public async Task<List<Administrador>> ListarAdm()
        {
            var lista = await _context.Administradores.ToListAsync();

            return lista;
        }
    }
}
