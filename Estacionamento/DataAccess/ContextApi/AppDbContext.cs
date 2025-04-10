using Estacionamento.Models;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.DataAccess.ContextApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RegistroEstacionamento> RegistrosEstacionamentos {  get; set; }
        public DbSet<TabelaValor> TabelaValores { get; set; }
    }
}
