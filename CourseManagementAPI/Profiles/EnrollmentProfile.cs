using AutoMapper;
using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.Profiles;

public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<Enrollment, ReadEnrollmentDto>();
    }
}
