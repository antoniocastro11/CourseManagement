using AutoMapper;
using CourseManagementAPI.Data.Dtos;
using CourseManagementAPI.Data.Dtos.Course;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.Profiles;

public class CourseProfile : Profile
{
    public CourseProfile() {
        CreateMap<Course, ReadCourseDto>();
        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();
    }
}
