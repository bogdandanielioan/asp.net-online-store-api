using AutoMapper;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Enrolments.Models;

namespace OnlineSchool.Enrolments.Mappings
{
    public class MappingProfileEnrolment : Profile
    {
        public MappingProfileEnrolment() {

            CreateMap<CreateRequestEnrolment, Enrolment>();
        }
    }
}
