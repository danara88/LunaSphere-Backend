using AutoMapper;
using LunaSphere.Application.Users.DTOs;
using LunaSphere.Domain.Users;

namespace LunaSphere.Application.Common.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region User
        CreateMap<User, UserDTO>();
        #endregion
    }
}