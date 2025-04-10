namespace Estacionamento.Models
{
    public class Administrador
    {
        public Administrador(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }

        public  Guid Id {get;set;}
        public string Nome {  get; set; }
        public string Email {  get; set; }
        public string Senha {  get; set; }
    }
}
