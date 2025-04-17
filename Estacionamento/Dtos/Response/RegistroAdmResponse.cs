namespace Estacionamento.Dtos.Response
{
    public class RegistroAdmResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool UsuarioValido { get; set; }
    }
}
