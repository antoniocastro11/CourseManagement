using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos;
using CourseManagementAPI.Data.Dtos.Course;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace CourseManagementAPI.Services;

public class CourseService (CourseDbContext context, IMapper mapper) 
{ 
    public async Task<ReadCourseDto> GetCourseAuxAsync(Guid id) {
        Course? course = await context.Courses.FindAsync(id);

        if (course is null) return null;

        ReadCourseDto response = mapper.Map<ReadCourseDto>(course);

        return response;
    }

    public async Task CreateCourse (CreateCourseDto dto)
    {
        Course course = mapper.Map<Course>(dto);
        
        context.Courses.Add(course);
        
        await context.SaveChangesAsync();
    }

    public async Task<List<ReadCourseDto>> GetAllCoursesAsync(int take = 10, int skip = 0, string? category = null)
    {
        if(category is null)
        {
            return mapper.Map<List<ReadCourseDto>>(context.Courses.Skip(skip).Take(take).ToListAsync());
        }
        return  mapper.Map<List<ReadCourseDto>>(context.Courses.Where(c => c.Category == category).Skip(skip).Take(take).ToListAsync());
    }

    public async Task UpdateCourse(Guid id, UpdateCourseDto dto)
    {
        var course = await context.Courses.FindAsync(id);
        mapper.Map(dto, course); // faço assim para preservar o objeto, se fizesse co, <>() estaria criando outro objeto
        await context.SaveChangesAsync();
    }
}
