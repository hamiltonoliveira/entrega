using Domain.Dto;
using Domain.Enums;
using Domain.Validation;
using FluentValidation;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;

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
        public TipoFuncionario Role { get; set; }
        public Pessoa Pessoa { get; set; }
        protected User() { }

        public User(string Cpf, Pessoa pessoa)
        {
            SetCpf(Cpf);
            Pessoa = pessoa;
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
            }
        }
    }
}
