using Estacionamento.Dtos.Request;
using FluentValidation;

namespace Estacionamento.Validator
{
    public class EdicaoEstacionamentoRegistroValidation : AbstractValidator<RegistroEstacionametoEdicaoRequest>
    {
        public EdicaoEstacionamentoRegistroValidation()
        {

            RuleFor(x => x.PlacaCarro).NotEmpty().WithMessage("Placa do veículo obrigatória.")
                .Length(7).WithMessage("A placa deve conter exatamente 7 caracteres.");
        }
    }
}
