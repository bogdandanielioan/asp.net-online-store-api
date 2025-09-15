using AutoMapper;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;

namespace OnlineSchool.Students.Mappings
{
    public class MappingProfilesStudent : Profile
    {
        public MappingProfilesStudent()
        {
            CreateMap<CreateRequestStudent, Student>();
        }
    }
}
