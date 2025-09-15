using AutoMapper;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.StudentCards.Models;

namespace OnlineSchool.Students.Mappings
{
    public class MappingProfilesStudent : Profile
    {
        public MappingProfilesStudent()
        {
            CreateMap<CreateRequestStudent, Student>();
            // Needed for RepositoryStudent.Create() where we map request to StudentCard
            CreateMap<CreateRequestStudent, StudentCard>();
        }
    }
}
