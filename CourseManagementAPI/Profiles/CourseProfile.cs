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
CreateMap<UpdateCourseDto, Course>()
    .ForMember(d => d.Title,       o => o.Condition(s => s.Title != null))
    .ForMember(d => d.Description, o => o.Condition(s => s.Description != null))
    .ForMember(d => d.Category,    o => o.Condition(s => s.Category != null))
    .ForMember(d => d.WorkLoad,    o => o.Condition(s => s.WorkLoad.HasValue))
    .ForMember(d => d.IsActive,    o => o.Condition(s => s.IsActive.HasValue));    }
}
