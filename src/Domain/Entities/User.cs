using Domain.Dto;
using Domain.Enums;
using Domain.Validation;
using FluentValidation;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Entities
{
    public class User : Base
    {

        public string? UserName { get; set; }
        [JsonIgnore]
        public string? Password { get; set; }
        public string? GuidI { get; set; } = Guid.NewGuid().ToString();
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public TipoUser Role { get; set; }
        public Pessoa Pessoa { get; set; }
        public Endereco Endereco { get; set; }
        public bool Ativo { get; set; }
        protected User() { }

        public User(string Cpf, Pessoa pessoa, Endereco endereco)
        {
            SetCpf(Cpf);
            Pessoa = pessoa;
            Endereco = endereco;
        }
        public void SetUserName(string email)
        {
            if (email != null)
            {
                string[] partes = email.Split('@');
                this.UserName = partes[0].ToLower();
            }
        }

        private void SetCpf(string cpf)
        {
            if (AtribuirCpf(cpf))
            {
                this.Cpf = cpf;
            }
            else
            {
                this.Cpf = null;
            }
        }
        private bool AtribuirCpf(string cpfNumero)
        {
            var cpf = new Cpf(cpfNumero);
            if (!cpf.Validar()) return false;
            return true;
        }

        public class UserValidation : AbstractValidator<UserDTO>
        {
            public UserValidation()
            {
                RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("O campo Name não pode ser nulo");
                RuleFor(x => x.Password).NotEmpty().WithMessage("O campo Password não pode ser nulo");
                RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("O campo de Email é obrigatório.").EmailAddress().WithMessage("O campo de email não possui um formato válido.");
                RuleFor(x => x.Cpf).NotNull().NotEmpty().WithMessage("O campo de Cpf é obrigatório.");
                RuleFor(x => x.Role).NotNull().NotEmpty().WithMessage("O campo Role não pode ser nulo");
                RuleFor(Pessoa => Pessoa.Celular).NotNull().WithMessage("O campo Celular não pode ser nulo"); 
                RuleFor(Pessoa => Pessoa.Foto).NotNull().WithMessage("O campo Foto não pode ser nulo");

                RuleFor(Endereco => Endereco.logradouro).NotNull().WithMessage("O campo Endereço não pode ser nulo");
                RuleFor(Endereco => Endereco.complemento).NotNull().WithMessage("O campo Complemento não pode ser nulo");
                RuleFor(Endereco => Endereco.numero).NotNull().WithMessage("O campo Número não pode ser nulo");
                RuleFor(Endereco => Endereco.cep).NotNull().WithMessage("O campo Cep não pode ser nulo");
                RuleFor(Endereco => Endereco.bairro).NotNull().WithMessage("O campo Bairro não pode ser nulo");
                RuleFor(Endereco => Endereco.cidade).NotNull().WithMessage("O campo Cidade não pode ser nulo");
                RuleFor(Endereco => Endereco.estado).NotNull().WithMessage("O campo Estado não pode ser nulo");
                RuleFor(Endereco => Endereco.municipio).NotNull().WithMessage("O campo Município não pode ser nulo");
            }
        }
    }
}
