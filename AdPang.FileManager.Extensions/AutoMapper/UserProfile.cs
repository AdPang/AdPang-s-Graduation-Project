using AdPang.FileManager.Models.IdentityEntities;
using AdPang.FileManager.Shared.Dtos;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}