namespace Domain.Entities
{
    public class Endereco
    {
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string numero { get; set; }
        public string cep { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string municipio { get; set; }

        public Endereco(string logradouro, string complemento, string numero, string cep, string bairro, string cidade, string estado, string municipio)
        {
            this.logradouro = logradouro;
            this.complemento = complemento;
            this.numero = numero;
            this.cep = cep;
            this.bairro = bairro;
            this.cidade = cidade;
            this.estado = estado;
            this.municipio = municipio;
        }
    }
}