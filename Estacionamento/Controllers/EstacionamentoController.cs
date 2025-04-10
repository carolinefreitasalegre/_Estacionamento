using Estacionamento.Dtos.Request;
using Estacionamento.Services;
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




        #region Metodos Views
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index(string? token = null)
        {
            ViewBag.Token = token;
            return View();
        }




        public IActionResult CadastrarVeiculo()
        {
            return View();
        }


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


        #endregion


        #region Metodos Lógica

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
        public async Task<IActionResult> FinalizarVaga(Guid Id)
        {
            var finalizar = await _service.FinalizarVaga(Id);

            if (finalizar == null)
            {
                return NotFound();
            }

            TempData["Mensagem"] = "Vaga finalizada com sucesso!";
            return View("AreaPagamento", finalizar);
        }

        [HttpGet]
        public async Task<IActionResult> ListaFinalizados()
        {
            var finalizados = await _service.ListarCarros();

            return View("ListaFinalizados", finalizados);
        }

        [HttpGet]
        public async Task<IActionResult> AreaPagamento()
        {
            return View("AreaPagamento");
        }


        #endregion




        // dialog para detalhar veiculo como hra de entrada,... com btn de anular e editar
        // ao clicar em editar 
    }
}


/*
    colocar:  asp-controller="Home" asp-action="Veiculos" 
    home = nome da controller
    action = nome da rota desejada

 

    quando nao colocatdo nenhum http, ele entende como um get

    para editar precisa da rota get e da rota post 

    

*/