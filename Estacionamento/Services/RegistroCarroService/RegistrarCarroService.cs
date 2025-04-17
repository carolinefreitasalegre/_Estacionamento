using Estacionamento.DataAccess.Repositories.RegistroCarroRepository;
using Estacionamento.Dtos.Request;
using Estacionamento.Models;
using Estacionamento.Services.PagamentoService;

namespace Estacionamento.Services.RegistroCarroService
{
    public class RegistrarCarroService : IRegistrarCarroService
    {

        private readonly IRegistrarCarroRepository _repository;
        private readonly EstacionamentoCalculoService _calculoService;

        public RegistrarCarroService(IRegistrarCarroRepository repository, EstacionamentoCalculoService calculoService)
        {
            _repository = repository;
            _calculoService = calculoService;
        }

        public async Task<List<RegistroEstacionamento>> ListarCarros()
        {
            return await _repository.ListarCarros();
        }

        public async Task<RegistroEstacionamento> RegistrarCarro(RegistroEstacionamentoRequest request)
        {




            try
            {
                var novo_registro = new RegistroEstacionamento()
                {
                    Id = Guid.NewGuid(),
                    PlacaCarro = request.PlacaCarro,
                    HorarioEntrada = DateTime.Now,
                };


                return await _repository.RegistrarCarro(novo_registro);

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar salvar registro", ex);
            }

        }

        public async Task<RegistroEstacionamento> BuscarRegistro(Guid Id)
        {
            try
            {
                var registro = await _repository.BuscarRegistro(Id) ?? throw new Exception("Registro não encontrado");

                registro.ValorPagar = _calculoService.CalcularValorPagar(registro.HorarioEntrada, registro.HorarioSaida);


                return registro;
            }
            catch (Exception ex)
            {

                throw new Exception("Algo deu errado ao buscar dados do registro.", ex);
            }
        }

        public async Task<RegistroEstacionamento> EditarRegistro(RegistroEstacionametoEdicaoRequest request)
        {
            var registro = await _repository.BuscarRegistro(request.Id) ?? throw new Exception("Registro não encontrado.");

            registro.PlacaCarro = request.PlacaCarro;

            return await _repository.EditarRegistro(registro);

        }

        public async Task<RegistroEstacionamento> EfetuarPagamento(Guid Id, RegistroEstacionametoEdicaoRequest request)
        {
            try
            {
                var registro = await _repository.BuscarRegistro(Id) ?? throw new Exception("Registro não encontrado");


                registro.HorarioSaida = DateTime.Now;


                var calculo = new EstacionamentoCalculoService();
                registro.ValorPagar = _calculoService.CalcularValorPagar(registro.HorarioEntrada, registro.HorarioSaida);

                if (registro.Pago == true)
                {
                    registro.Finalizado = true;
                }
                else
                {
                    registro.Finalizado = false;
                }


                await _repository.EditarRegistro(registro);

                return registro;

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar finalizar registro não encontrado", ex);
            }
        }


    }
}
