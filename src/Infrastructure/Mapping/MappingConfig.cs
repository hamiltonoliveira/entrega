using AutoMapper;
using Domain.Dto;
using Domain.Entities;

namespace Infrastructure.Mapping
{
    public class MappingConfig : Profile
    {
        public class DomainDTOMapping : Profile
        {
            public DomainDTOMapping()
            {
                CreateMap<User, UserDTO>();
                CreateMap<UserDTO, User>();
            }
        }
    }
}
