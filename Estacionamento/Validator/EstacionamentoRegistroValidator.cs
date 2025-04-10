using Estacionamento.DataAccess.ContextApi;
using Estacionamento.Dtos.Request;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Validator
{
    public class EstacionamentoRegistroValidator : AbstractValidator<RegistroEstacionamentoRequest>
    {
        private readonly AppDbContext _context;

        public EstacionamentoRegistroValidator(AppDbContext context )
        {
            _context = context;

            RuleFor(x => x.PlacaCarro).NotEmpty().WithMessage("Placa do veículo obrigatória.")
                .Length(7).WithMessage("A placa deve conter exatamente 7 caracteres.")
                .MustAsync(PlacaUnica).WithMessage("Placa já registrada.");
        }

        public async Task<bool> PlacaUnica(string placa, CancellationToken token)
        {
            return !await _context.RegistrosEstacionamentos.AnyAsync(p => p.PlacaCarro == placa);

        }
    }
}
