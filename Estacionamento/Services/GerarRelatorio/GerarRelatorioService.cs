
using System.Drawing;
using Estacionamento.DataAccess.ContextApi;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Estacionamento.Services.GerarRelatorio
{
    public class GerarRelatorioService : IGerarRelatorioService
    {

        private readonly AppDbContext _context;

        public GerarRelatorioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GerarRelatorio()
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var dados = await _context.RegistrosEstacionamentos.ToListAsync();

            using (var pack = new ExcelPackage())
            {
                var worksheet = pack.Workbook.Worksheets.Add("Relatório estacionamento");

                worksheet.Cells[1, 1].Value = "Placa do Carro";
                worksheet.Cells[1, 2].Value = "Horario de Entrada";
                worksheet.Cells[1, 3].Value = "Horario de Saída";
                worksheet.Cells[1, 4].Value = "Valor Pago";

                using (var range = worksheet.Cells[1,1,1,4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int linha = 2;

                foreach(var dado in dados)
                {
                    worksheet.Cells[linha, 1].Value = dado.PlacaCarro;
                    worksheet.Cells[linha, 2].Value = dado.HorarioEntrada.ToString("dd/mm/yyyy/hh-mm");
                    worksheet.Cells[linha, 3].Value = dado.HorarioSaida.ToString();
                    worksheet.Cells[linha, 4].Value = dado.ValorPagar;

                    linha++;

                }

                worksheet.Protection.IsProtected = true;
                worksheet.Protection.SetPassword("DadosInalteraveis123##");
                worksheet.Cells.AutoFitColumns();

                return pack.GetAsByteArray();
            }




        }
    }
}
