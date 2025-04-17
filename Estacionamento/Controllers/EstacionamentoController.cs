using Estacionamento.Dtos.Request;
using Estacionamento.Services.FactoryMethod;
using Estacionamento.Services.RegistroCarroService;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Controllers
{
    [Authorize]
    public class EstacionamentoController : Controller
    {
        private readonly ILogger<EstacionamentoController> _logger;
        private readonly IRegistrarCarroService _service;
        private readonly IValidator<RegistroEstacionamentoRequest> _validator;
        private readonly IValidator<RegistroEstacionametoEdicaoRequest> _validatorEdicao;

        public EstacionamentoController(ILogger<EstacionamentoController> logger, IRegistrarCarroService service, IValidator<RegistroEstacionamentoRequest> validator, IValidator<RegistroEstacionametoEdicaoRequest> validatorEdicao)
        {
            _logger = logger;
            _service = service;
            _validator = validator;
            _validatorEdicao = validatorEdicao;
        }



        public IActionResult Index(string? token = null)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpGet]
        public IActionResult CadastrarVeiculo()
        {
            return View();
        }

        [HttpGet]
        private async Task<RegistroEstacionametoEdicaoRequest> BuscarRegistroParaEdicao(Guid id)
        {
            var registro = await _service.BuscarRegistro(id)
                ?? throw new Exception("Registro não encontrado.");

            return new RegistroEstacionametoEdicaoRequest
            {
                Id = registro.Id,
                PlacaCarro = registro.PlacaCarro,
            };
        }


        [HttpGet]
        public async Task<IActionResult> EditarVeiculo(RegistroEstacionametoEdicaoRequest request)
        {
            var dto = await BuscarRegistroParaEdicao(request.Id);
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> ListaFinalizados()
        {
            var finalizados = await _service.ListarCarros();

            return View("ListaFinalizados", finalizados);
        }

        [HttpGet]
        public async Task<IActionResult> Veiculos()
        {
            var lista = await _service.ListarCarros();

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(Guid Id)
        {
            var auto = await _service.BuscarRegistro(Id);

            if (auto == null)
                return NotFound();

            return PartialView("_DetalhesVeiculo", auto);

        }


        [HttpGet]
        public async Task<IActionResult> EfetuarPagamento(RegistroEstacionametoEdicaoRequest request)
        {
            var registro = await _service.BuscarRegistro(request.Id);


            if (registro == null)
                return NotFound("Registro não encontrado");

            return View("EfetuarPagamento", registro);
        }




        [HttpPost]
        public async Task<IActionResult> CadastrarVeiculo(RegistroEstacionamentoRequest request)
        {

            try
            {

                var validarRegistro = await _validator.ValidateAsync(request);


                if (!validarRegistro.IsValid)
                {
                    foreach (var erro in validarRegistro.Errors)
                    {
                        ModelState.AddModelError(erro.PropertyName, erro.ErrorMessage);
                    }

                    return View(request);
                }

                var registro = await _service.RegistrarCarro(request);

                TempData["Mensagem"] = "Veículo registrado com sucesso!";

                return RedirectToAction("Veiculos");

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorre um erro ao tentar registrar carro", ex);
            }
        }
     

        [HttpPost]
        public async Task<IActionResult> EditarVeiculo(Guid Id, RegistroEstacionametoEdicaoRequest request)
        {
            try
            {

                var validarRegistro = _validatorEdicao.Validate(request);

                if (!validarRegistro.IsValid)
                {
                    foreach (var erro in validarRegistro.Errors)
                    {
                        ModelState.AddModelError(erro.PropertyName, erro.ErrorMessage);
                    }

                    return View(request);
                }

                var registro = await _service.BuscarRegistro(request.Id);


                if (registro == null)
                {
                    return NotFound();
                }

                await _service.EditarRegistro(request);


                TempData["Mensagem"] = "Veículo editado com sucesso!";
                return RedirectToAction("Veiculos");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar alteração", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EfetuarPagamento(Guid Id, RegistroEstacionametoEdicaoRequest request)
        {
            var registro = await _service.BuscarRegistro(Id);

            if (registro == null)
            {
                return NotFound("Registro não encontrado.");
            }

            try
            {
                registro.MetodoPagamento = request.MetodoPagamento;

                var pagamento = new RegistroEstacionametoEdicaoRequest
                {
                    HorarioEntrada = registro.HorarioEntrada,
                    HorarioSaida = DateTime.Now,
                    MetodoPagamento = request.MetodoPagamento
                };



                var pagamentoService = MetodoPagamentoFactory.GerarMetodoPagamento(request.MetodoPagamento);
                var registroPago = await pagamentoService.ProcessarPagamento(registro);

                await _service.EfetuarPagamento(Id, pagamento);

                TempData["Mensagem"] = "Vaga finalizada com sucesso!";
                return RedirectToAction("ListaFinalizados", registroPago);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


    }
}

