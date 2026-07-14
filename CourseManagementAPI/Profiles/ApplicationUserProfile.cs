using AutoMapper;
using CourseManagementAPI.Data.Dtos.Auth;

namespace CourseManagementAPI.Profiles;

public class ApplicationUserProfile : Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<ApplicationUserProfile, RegisterDto>();
    }
}
