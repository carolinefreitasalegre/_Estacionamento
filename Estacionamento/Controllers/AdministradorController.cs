using Estacionamento.Dtos.Request;
using Estacionamento.Dtos.Response;
using Estacionamento.Services.RegistroAdmService;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Controllers
{
    [Authorize]
    public class AdministradorController : Controller
    {
        private readonly ILogger<EstacionamentoController> _logger;

        private readonly IRegistrarAdmService _service;
        private readonly IValidator<RegistroAdmRequest> _validator;
        private readonly IValidator<RegistroAdmEdicaoRequest> _validatorEdit;

        public AdministradorController(ILogger<EstacionamentoController> logger, IRegistrarAdmService service, IValidator<RegistroAdmRequest> validator, IValidator<RegistroAdmEdicaoRequest> validatorEdit)
        {
            _logger = logger;
            _service = service;
            _validator = validator;
            _validatorEdit = validatorEdit;
        }

        public IActionResult Index(string? token = null)
        {
            return View();
        }

        [HttpGet]
        public IActionResult CadastrarAdm()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Administradores(RegistroAdmResponse response)
        {
            var lista = await _service.ListarAdm();

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> EditarAdm(Guid Id)
        {
            var adm = await _service.BuscarAdm(Id);

            if (adm == null)
                return BadRequest("Administrador não ecnontrado");


            var viewModel = new RegistroAdmEdicaoRequest
            {
                Id = adm.Id,
                Nome = adm.Nome,
                Email = adm.Email,
                Senha = adm.Senha,
                UsuarioValido = adm.UsuarioValido
            };

            return PartialView("_EditarAdm", viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> EditarAdm(Guid Id, RegistroAdmEdicaoRequest request)
        {

            try
            {
                var adm = await _service.BuscarAdm(Id);

                if (adm == null)
                    return NotFound("Administrador não encontrado");


                var validator = await _validatorEdit.ValidateAsync(request);

                if (!validator.IsValid)
                {
                    TempData["Mensagem"] = "Erro ao editar administrador, Revise os dados e tente novame!";

                    return View(request);
                }

                await _service.EditarAdm(Id, request);

                TempData["Mensagem"] = "Administrador editado com sucesso!";
                return RedirectToAction("Administradores");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Erro ao buscar administrador...");
                return StatusCode(500, "Erro interno ao processar a requisição");
            }



            

        }


        [HttpPost]
        public async Task<IActionResult> CadastrarAdm(RegistroAdmRequest request)
        {
            try
            {
                var validator = await _validator.ValidateAsync(request);

                if (!validator.IsValid)
                {
                    foreach (var erro in validator.Errors)
                    {
                        ModelState.AddModelError(erro.PropertyName, erro.ErrorMessage);
                    }

                    return View(request);
                }

                var adm = await _service.CadastrarAdm(request);
                TempData["Mensagem"] = "Administrador cadastrado com sucesso!";
                return RedirectToAction("Administradores");
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao tentar cadastrar o administrador.", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> DesabilitarAdmin(Guid Id)
        {
            var adm = _service.BuscarAdm(Id);

            if (adm == null)
                return BadRequest("Administrador não encontrado.");

            await _service.DesabilitarAdm(Id);

            return View(adm);
        }

        [HttpPost]
        public async Task<IActionResult> BuscarAdm(Guid Id)
        {
            var adm = await _service.BuscarAdm(Id);

            if (adm == null)
                return NotFound();

            return View(adm);
        }

        }
    }
