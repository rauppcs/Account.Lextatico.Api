using AutoMapper;
using Account.Lextatico.Application.Dtos.User;
using Account.Lextatico.Domain.Models;

namespace Account.Lextatico.Application.AutoMapper.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            // DTO TO MODEL

            // MODEL TO DTO
            CreateMap<ApplicationUser, UserDetailDto>();
        }
    }
}
