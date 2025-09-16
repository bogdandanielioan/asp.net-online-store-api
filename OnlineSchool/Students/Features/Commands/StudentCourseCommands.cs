using MediatR;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Services.interfaces;

namespace OnlineSchool.Students.Features.Commands;

public record EnrollStudentInCourseCommand(string StudentId, string CourseName) : IRequest<Student>;

public class EnrollStudentInCourseCommandHandler : IRequestHandler<EnrollStudentInCourseCommand, Student>
{
    private readonly ICommandServiceStudent _studentCommandService;
    private readonly IQueryServiceCourse _courseQueryService;

    public EnrollStudentInCourseCommandHandler(
        ICommandServiceStudent studentCommandService,
        IQueryServiceCourse courseQueryService)
    {
        _studentCommandService = studentCommandService;
        _courseQueryService = courseQueryService;
    }

    public async Task<Student> Handle(EnrollStudentInCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseQueryService.GetByName(request.CourseName);
        return await _studentCommandService.EnrollmentCourse(request.StudentId, course);
    }
}

public record UnenrollStudentFromCourseCommand(string StudentId, string CourseName) : IRequest<Student>;

public class UnenrollStudentFromCourseCommandHandler : IRequestHandler<UnenrollStudentFromCourseCommand, Student>
{
    private readonly ICommandServiceStudent _studentCommandService;
    private readonly IQueryServiceCourse _courseQueryService;

    public UnenrollStudentFromCourseCommandHandler(
        ICommandServiceStudent studentCommandService,
        IQueryServiceCourse courseQueryService)
    {
        _studentCommandService = studentCommandService;
        _courseQueryService = courseQueryService;
    }

    public async Task<Student> Handle(UnenrollStudentFromCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseQueryService.GetByName(request.CourseName);
        return await _studentCommandService.UnEnrollmentCourse(request.StudentId, course);
    }
}
