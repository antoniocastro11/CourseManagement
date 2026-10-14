using AutoMapper;
using CourseManagementAPI.Data.Dtos.Auth;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.Profiles;

public class ApplicationUserProfile : Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<RegisterDto, ApplicationUser>()
        .ForMember(u => u.UserName, o => o.MapFrom(r => r.Email));;
    }
}
