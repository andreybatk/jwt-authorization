using AutoMapper;
using JwtAuthorization.API.Contracts;
using JwtAuthorization.DB.Entities;

namespace JwtAuthorization.API.Mapper
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<RegisterRequest, User>();
        }
    }
}