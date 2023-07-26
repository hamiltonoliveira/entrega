using Application.Interfaces;
using Domain.Dto;
using Infrastructure.Interfaces;

namespace Application.Services
{

    public class AutenticarService : IAutenticarService
    {
        private readonly IAutenticarRepositorio _autenticarRepositorio;
        public AutenticarService(IAutenticarRepositorio autenticarRepositorio)
        {
            _autenticarRepositorio = autenticarRepositorio;
        }

        public async Task<TokensDto> GerarToKen(string Email)
        {
            TokensDto tokens = await _autenticarRepositorio.GerarToKen(Email);
            return tokens;
        }
    }
}
