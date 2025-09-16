using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Models;
using OnlineSchool.Students.Controllers;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Features.Commands;
using OnlineSchool.Students.Features.Queries;
using OnlineSchool.Students.Models;
using OnlineSchool.System.Exceptions;

namespace Teste.Unit;

public class StudentsControllerUnitTests
{
    private static ControllerStudent CreateController(Mock<IMediator> mediator)
    {
        return new ControllerStudent(mediator.Object);
    }

    [Fact]
    public async Task GetStudents_ReturnsOk_WithList()
    {
        var mediator = new Mock<IMediator>();
        var items = new List<DtoStudentView> { new DtoStudentView { Id = "s1", Name = "John", Email = "john@ex.com", Age = 20 } };
        mediator.Setup(m => m.Send(It.IsAny<GetStudentsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(items);
        var ctrl = CreateController(mediator);

        var result = await ctrl.GetStudents();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<List<DtoStudentView>>(ok.Value);
        Assert.Single(value);
    }

    [Fact]
    public async Task GetStudents_ReturnsNotFound_WhenEmpty()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetStudentsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemsDoNotExist("none"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.GetStudents();

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetByName_ReturnsOk_WhenFound()
    {
        var mediator = new Mock<IMediator>();
        var dto = new DtoStudentView { Id = "s2", Name = "Jane", Email = "jane@ex.com", Age = 21 };
        mediator.Setup(m => m.Send(It.Is<GetStudentByNameQuery>(q => q.Name == "Jane"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);
        var ctrl = CreateController(mediator);

        var result = await ctrl.GetByName("Jane");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<DtoStudentView>(ok.Value);
        Assert.Equal("s2", value.Id);
    }

    [Fact]
    public async Task GetByName_ReturnsNotFound_WhenMissing()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetStudentByNameQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("notfound"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.GetByName("Ghost");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var mediator = new Mock<IMediator>();
        var dto = new DtoStudentView { Id = "s3", Name = "Mike", Email = "mike@ex.com", Age = 25 };
        mediator.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == "s3"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);
        var ctrl = CreateController(mediator);

        var result = await ctrl.GetById("s3");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<DtoStudentView>(ok.Value);
        Assert.Equal("Mike", value.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.GetById("unknown");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetStudentCard_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var card = new OnlineSchool.StudentCards.Models.StudentCard { Id = "c1", IdStudent = "s1", Namecard = "card" };
        mediator.Setup(m => m.Send(It.Is<GetStudentCardQuery>(q => q.Id == "s1"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);
        var ctrl = CreateController(mediator);

        var result = await ctrl.GetStudentCard("s1");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(card, ok.Value);
    }

    [Fact]
    public async Task GetStudentCard_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetStudentCardQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.GetStudentCard("sX");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateStudent_ReturnsOk_WhenValid()
    {
        var mediator = new Mock<IMediator>();
        var create = new CreateRequestStudent { Name = "Neo", Email = "neo@ex.com", Age = 30 };
        var model = new Student { Id = "s4", Name = create.Name, Email = create.Email, Age = create.Age, UpdateDate = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<CreateStudentCommand>(c => c.Request == create), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);
        var ctrl = CreateController(mediator);

        var result = await ctrl.CreateStudent(create);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<DtoStudentView>(ok.Value);
        Assert.Equal("s4", value.Id);
    }

    [Fact]
    public async Task CreateStudent_ReturnsBadRequest_OnInvalidAge()
    {
        var mediator = new Mock<IMediator>();
        var create = new CreateRequestStudent { Name = "Neo", Email = "neo@ex.com", Age = 0 };
        mediator.Setup(m => m.Send(It.Is<CreateStudentCommand>(c => c.Request == create), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidAge("bad"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.CreateStudent(create);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateStudent_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var update = new UpdateRequestStudent { Name = "New" };
        var model = new Student { Id = "s5", Name = "New", Email = "e@e.com", Age = 18, UpdateDate = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<UpdateStudentCommand>(c => c.Id == "s5" && c.Request == update), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);
        var ctrl = CreateController(mediator);

        var result = await ctrl.UpdateStudent("s5", update);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<DtoStudentView>(ok.Value);
        Assert.Equal("s5", value.Id);
    }

    [Fact]
    public async Task UpdateStudent_ReturnsBadRequest_OnInvalidAge()
    {
        var mediator = new Mock<IMediator>();
        var update = new UpdateRequestStudent { Age = 0 };
        mediator.Setup(m => m.Send(It.Is<UpdateStudentCommand>(c => c.Id == "s5" && c.Request == update), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidAge("bad"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.UpdateStudent("s5", update);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateStudent_ReturnsNotFound_OnMissing()
    {
        var mediator = new Mock<IMediator>();
        var update = new UpdateRequestStudent { Name = "X" };
        mediator.Setup(m => m.Send(It.Is<UpdateStudentCommand>(c => c.Id == "missing" && c.Request == update), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.UpdateStudent("missing", update);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteStudent_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var model = new Student { Id = "s6", Name = "Del", Email = "d@e.com", Age = 19, UpdateDate = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<DeleteStudentCommand>(c => c.Id == "s6"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);
        var ctrl = CreateController(mediator);

        var result = await ctrl.DeleteStudent("s6");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteStudent_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<DeleteStudentCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.DeleteStudent("missing");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateBookForStudent_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var req = new BookCreateDTO { Name = "Book1", Created_at = DateTime.UtcNow };
        var model = new Student { Id = "s7", Name = "B", Email = "b@e.com", Age = 20, UpdateDate = DateTime.UtcNow, StudentBooks = new List<Book>() };
        mediator.Setup(m => m.Send(It.Is<CreateStudentBookCommand>(c => c.StudentId == "s7" && c.Request == req), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);
        var ctrl = CreateController(mediator);

        var result = await ctrl.CreateBookForStudent("s7", req);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateBookForStudent_ReturnsNotFound_OnMissingStudent()
    {
        var mediator = new Mock<IMediator>();
        var req = new BookCreateDTO { Name = "Book1", Created_at = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<CreateStudentBookCommand>(c => c.StudentId == "missing"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.CreateBookForStudent("missing", req);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateBookForStudent_ReturnsBadRequest_OnInvalidName()
    {
        var mediator = new Mock<IMediator>();
        var req = new BookCreateDTO { Name = "", Created_at = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<CreateStudentBookCommand>(c => c.StudentId == "s7"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidName("bad"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.CreateBookForStudent("s7", req);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateBookForStudent_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var req = new BookUpdateDTO { Name = "B2", Created_at = DateTime.UtcNow };
        var model = new Student { Id = "s8", Name = "U", Email = "u@e.com", Age = 22, UpdateDate = DateTime.UtcNow, StudentBooks = new List<Book>() };
        mediator.Setup(m => m.Send(It.Is<UpdateStudentBookCommand>(c => c.StudentId == "s8" && c.BookId == "b1" && c.Request == req), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);
        var ctrl = CreateController(mediator);

        var result = await ctrl.UpdateBookForStudent("s8", "b1", req);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateBookForStudent_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();
        var req = new BookUpdateDTO { Name = "B2", Created_at = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<UpdateStudentBookCommand>(c => c.StudentId == "s8" && c.BookId == "missing"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.UpdateBookForStudent("s8", "missing", req);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateBookForStudent_ReturnsBadRequest_OnInvalidName()
    {
        var mediator = new Mock<IMediator>();
        var req = new BookUpdateDTO { Name = "", Created_at = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<UpdateStudentBookCommand>(c => c.StudentId == "s8"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidName("bad"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.UpdateBookForStudent("s8", "b1", req);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteBookForStudent_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var model = new Student { Id = "s9", Name = "D", Email = "d@e.com", Age = 23, UpdateDate = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<DeleteStudentBookCommand>(c => c.StudentId == "s9" && c.BookId == "b1"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);
        var ctrl = CreateController(mediator);

        var result = await ctrl.DeleteBookForStudent("s9", "b1");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteBookForStudent_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.Is<DeleteStudentBookCommand>(c => c.StudentId == "s9" && c.BookId == "missing"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.DeleteBookForStudent("s9", "missing");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task EnrollmentCourse_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var model = new Student { Id = "s10", Name = "E", Email = "e@e.com", Age = 26, UpdateDate = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<EnrollStudentInCourseCommand>(c => c.StudentId == "s10" && c.CourseName == "Math"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);
        var ctrl = CreateController(mediator);

        var result = await ctrl.EnrollmentCourse("s10", "Math");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task EnrollmentCourse_ReturnsNotFound_WhenCourseMissing()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.Is<EnrollStudentInCourseCommand>(c => c.StudentId == "s10" && c.CourseName == "MissingCourse"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundCourse("missing"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.EnrollmentCourse("s10", "MissingCourse");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task EnrollmentCourse_ReturnsBadRequest_OnInvalidCourse()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.Is<EnrollStudentInCourseCommand>(c => c.StudentId == "s10" && c.CourseName == "Math"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidCourse("bad"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.EnrollmentCourse("s10", "Math");

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UnEnrollmentCourse_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var model = new Student { Id = "s11", Name = "U", Email = "u@e.com", Age = 27, UpdateDate = DateTime.UtcNow };
        mediator.Setup(m => m.Send(It.Is<UnenrollStudentFromCourseCommand>(c => c.StudentId == "s11" && c.CourseName == "Math"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);
        var ctrl = CreateController(mediator);

        var result = await ctrl.UnEnrollmentCourse("s11", "Math");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task UnEnrollmentCourse_ReturnsNotFound_WhenCourseMissing()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.Is<UnenrollStudentFromCourseCommand>(c => c.StudentId == "s11" && c.CourseName == "Missing"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundCourse("missing"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.UnEnrollmentCourse("s11", "Missing");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UnEnrollmentCourse_ReturnsBadRequest_OnInvalid()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.Is<UnenrollStudentFromCourseCommand>(c => c.StudentId == "s11" && c.CourseName == "Math"), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidCourse("bad"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.UnEnrollmentCourse("s11", "Math");

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UnEnrollmentCourse_ReturnsNotFound_OnMissingStudent()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<UnenrollStudentFromCourseCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController(mediator);

        var result = await ctrl.UnEnrollmentCourse("missing", "Math");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
