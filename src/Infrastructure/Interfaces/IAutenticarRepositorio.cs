using Domain.Dto;

namespace Infrastructure.Interfaces
{
    public interface IAutenticarRepositorio
    {
        Task<TokensDto> GerarToKen(string GuidId);
    }
}
