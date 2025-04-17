using Estacionamento.DataAccess.ContextApi;
using Estacionamento.Models;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.DataAccess.Repositories.RegistroCarroRepository
{
    public class RegistrarCarroRepository : IRegistrarCarroRepository
    {

        private readonly AppDbContext _context;

        public RegistrarCarroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RegistroEstacionamento> RegistrarCarro(RegistroEstacionamento request)
        {
            _context.RegistrosEstacionamentos.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<List<RegistroEstacionamento>> ListarCarros()
        {
            return await _context.RegistrosEstacionamentos.ToListAsync();

        }


        public async Task<RegistroEstacionamento> EditarRegistro(RegistroEstacionamento request)
        {
            _context.RegistrosEstacionamentos.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }


        public async Task<RegistroEstacionamento> EfetuarPagamento(Guid Id, RegistroEstacionamento request)
        {
            var registro = _context.RegistrosEstacionamentos.FirstOrDefault(r => r.Id == Id) ?? throw new Exception("Registro não encontrado.");

            _context.RegistrosEstacionamentos.Update(registro);
            await _context.SaveChangesAsync();
            return request;

        }

        public async Task<RegistroEstacionamento> BuscarRegistro(Guid Id)
        {

            var registro = await _context.RegistrosEstacionamentos.FirstOrDefaultAsync(x => x.Id == Id)
                ?? throw new KeyNotFoundException("Registro nao ecncontrado.");

            return registro;

        }







    }
}
