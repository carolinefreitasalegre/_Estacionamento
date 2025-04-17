using Estacionamento.DataAccess.ContextApi;
using Estacionamento.Dtos.Request;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Validator
{
    public class EstacionamentoRegistroAdminValidator : AbstractValidator<RegistroAdmRequest>
    {
        private readonly AppDbContext _context;

        public EstacionamentoRegistroAdminValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(e => e.Nome).NotEmpty().WithMessage("Campo nome não pode estar em branco");
            RuleFor(e => e.Email).NotEmpty().WithMessage("Campo email não pode estar em branco").MustAsync(EmailUnico).WithMessage("Email já cadastrado");
            RuleFor(e => e.Senha).NotEmpty().WithMessage("Campo senha não pode estar em branco")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres")
                .Matches(@"[A-Za-z]").WithMessage("A senha deve conter letras")
                .Matches(@"\d").WithMessage("A senha deve conter ao menos um número")
                .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithMessage("A senha deve conter ao menos um caractere especial"); ;
        }

        public async Task<bool> EmailUnico(string email, CancellationToken token)
        {
            return !await _context.Administradores.AnyAsync(a => a.Email == email, token);
        }
    }
}
