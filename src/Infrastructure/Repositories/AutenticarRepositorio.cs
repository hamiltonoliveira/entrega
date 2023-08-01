using Domain.Dto;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AutenticarRepositorio : IAutenticarRepositorio
    {
        private readonly IUserRepository _userRepository;
        public AutenticarRepositorio(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<TokensDto> GerarToKen(string email)
        {
            List<User> usuarios = await _userRepository.WhereAsync(x => x.Email == email);

            var _UserName = string.Empty;
            var _GuidI = string.Empty;
            var _Role = string.Empty;

            foreach (var item in usuarios)
            {
                _UserName = item.UserName;
                _GuidI = item.GuidI;
                _Role = Enum.GetName(typeof(TipoFuncionario), item.Role);
            }

            var _Token = generateJwtToken(_UserName, _GuidI, _Role);
            var _TokenRefresh = generateJwtTokenRefresh(_GuidI, _Role);

            return new TokensDto()
            {
                Token = _Token,
                TokenRefresh = _TokenRefresh,
                Role = _Role
            };
        }

        private string generateJwtToken(string UserName, string guid, string Role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string oculto = CodigoCripto.Cripto();
            var key = Encoding.ASCII.GetBytes(oculto);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("UserName", UserName),
                    new Claim("Guid", guid),
                    new Claim(ClaimTypes.Role, Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string generateJwtTokenRefresh(string guid, string Role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string oculto = CodigoCripto.Cripto();
            var key = Encoding.ASCII.GetBytes(oculto);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                   new Claim("Guid", guid),
                   new Claim(ClaimTypes.Role, Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
