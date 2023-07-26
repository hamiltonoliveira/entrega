using Domain.Dto;

namespace Application.Interfaces
{
    public interface IAutenticarService
    {
        Task<TokensDto> GerarToKen(string GuidId);
    }
}
