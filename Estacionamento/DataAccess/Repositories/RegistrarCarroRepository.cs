using Estacionamento.DataAccess.ContextApi;
using Estacionamento.DataAccess.Contratos;
using Estacionamento.Models;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.DataAccess.Repositories
{
    public class RegistrarCarroRepository : IRegistrarCarroRepository
    {

        private readonly AppDbContext _context;

        public  RegistrarCarroRepository(AppDbContext context)
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


        public async Task<RegistroEstacionamento> FinalizarVaga(Guid Id)
        {
            var registro = await _context.RegistrosEstacionamentos.FirstOrDefaultAsync(x => x.Id == Id)
                ?? throw new KeyNotFoundException("Registro não encontrado");

            registro.Finalizado = true;


            _context.RegistrosEstacionamentos.Update(registro);
            await _context.SaveChangesAsync();

            return registro;

        }

        public async Task<RegistroEstacionamento> BuscarRegistro(Guid Id)
        {
            try
            {
                var registro = await _context.RegistrosEstacionamentos.FirstOrDefaultAsync(x => x.Id == Id) 
                    ?? throw new KeyNotFoundException("Registro nao ecncontrado.");

                return registro;
            }
            catch (Exception ex)
            {

                throw new KeyNotFoundException("Erro ao buscar registro.", ex);
            }
        }


       
    
    
    
    
    }
}
