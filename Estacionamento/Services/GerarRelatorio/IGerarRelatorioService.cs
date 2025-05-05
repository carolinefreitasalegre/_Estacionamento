namespace Estacionamento.Services.GerarRelatorio
{
    public interface IGerarRelatorioService
    {
        Task<byte[]> GerarRelatorio();
    }
}
