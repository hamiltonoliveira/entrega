namespace Domain.Entities
{
    public class Pessoa
    {
        public string? Celular { get; private set; }
        public string? Foto { get; private set;}
        public Pessoa(string? celular, string? foto)
        {
            Celular = celular;
            Foto = foto;
        }
    }
}