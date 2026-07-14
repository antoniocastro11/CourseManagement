using AutoMapper;
using CourseManagementAPI.Data.Dtos;
using CourseManagementAPI.Data.Dtos.Student;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.Profiles;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<CreateStudentDto, Student>();
        CreateMap<UpdateStudentDto, Student>();
        CreateMap<Student, ReadStudentDto>();
    }
}
