using AutoMapper;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;

namespace OnlineSchool.Courses.Mappings
{
    public class MappingProfileCourse : Profile
    {
        public MappingProfileCourse() {

            CreateMap<CreateRequestCourse, Course>();
        }
    }
}
