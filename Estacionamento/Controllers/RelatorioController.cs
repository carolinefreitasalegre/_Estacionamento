using Estacionamento.Services.GerarRelatorio;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly IGerarRelatorioService _gerarRelatorioService;

        public RelatorioController(IGerarRelatorioService gerarRelatorioService)
        {
            _gerarRelatorioService = gerarRelatorioService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GerarRelatorio()
        {
            var docExcel = await _gerarRelatorioService.GerarRelatorio();
            var nomeArquivo = $"Relatorio_vagas_fibalizadas_:{DateTime.Now:dd/MM}.xlsx";


            return File(docExcel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nomeArquivo);
        }
    }
}
