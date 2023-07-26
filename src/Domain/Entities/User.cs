using Domain.Enums;
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

    }
}
