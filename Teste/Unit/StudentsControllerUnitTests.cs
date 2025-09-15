using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Models;
using OnlineSchool.Students.Controllers;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Services.interfaces;
using OnlineSchool.System.Exceptions;

namespace Teste.Unit;

public class StudentsControllerUnitTests
{
    private readonly Mock<IQueryServiceStudent> _queryMock = new();
    private readonly Mock<ICommandServiceStudent> _commandMock = new();
    private readonly Mock<OnlineSchool.Courses.Services.interfaces.IQueryServiceCourse> _courseQueryMock = new();

    private ControllerStudent CreateController()
    {
        return new ControllerStudent(_queryMock.Object, _commandMock.Object, _courseQueryMock.Object);
    }

    [Fact]
    public async Task GetStudents_ReturnsOk_WithList()
    {
        // Arrange
        var items = new List<DtoStudentView> { new DtoStudentView { Id = "s1", Name = "John", Email = "john@ex.com", Age = 20 } };
        _queryMock.Setup(q => q.GetAll()).ReturnsAsync(items);
        var ctrl = CreateController();

        // Act
        var result = await ctrl.GetStudents();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<List<DtoStudentView>>(ok.Value);
        Assert.Single(value);
    }

    [Fact]
    public async Task GetStudents_ReturnsNotFound_WhenEmpty()
    {
        _queryMock.Setup(q => q.GetAll()).ThrowsAsync(new ItemsDoNotExist("none"));
        var ctrl = CreateController();

        var result = await ctrl.GetStudents();

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetByName_ReturnsOk_WhenFound()
    {
        var dto = new DtoStudentView { Id = "s2", Name = "Jane", Email = "jane@ex.com", Age = 21 };
        _queryMock.Setup(q => q.GetByNameAsync("Jane")).ReturnsAsync(dto);
        var ctrl = CreateController();

        var result = await ctrl.GetByName("Jane");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<DtoStudentView>(ok.Value);
        Assert.Equal("s2", value.Id);
    }

    [Fact]
    public async Task GetByName_ReturnsNotFound_WhenMissing()
    {
        _queryMock.Setup(q => q.GetByNameAsync(It.IsAny<string>())).ThrowsAsync(new ItemDoesNotExist("notfound"));
        var ctrl = CreateController();

        var result = await ctrl.GetByName("Ghost");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var dto = new DtoStudentView { Id = "s3", Name = "Mike", Email = "mike@ex.com", Age = 25 };
        _queryMock.Setup(q => q.GetById("s3")).ReturnsAsync(dto);
        var ctrl = CreateController();

        var result = await ctrl.GetById("s3");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<DtoStudentView>(ok.Value);
        Assert.Equal("Mike", value.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        _queryMock.Setup(q => q.GetById(It.IsAny<string>())).ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController();

        var result = await ctrl.GetById("unknown");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetStudentCard_ReturnsOk()
    {
        var card = new OnlineSchool.StudentCards.Models.StudentCard { Id = "c1", IdStudent = "s1", Namecard = "card" };
        _queryMock.Setup(q => q.CardById("s1")).ReturnsAsync(card);
        var ctrl = CreateController();

        var result = await ctrl.GetStudentCard("s1");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(card, ok.Value);
    }

    [Fact]
    public async Task GetStudentCard_ReturnsNotFound()
    {
        _queryMock.Setup(q => q.CardById(It.IsAny<string>())).ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController();

        var result = await ctrl.GetStudentCard("sX");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateStudent_ReturnsOk_WhenValid()
    {
        var create = new CreateRequestStudent { Name = "Neo", Email = "neo@ex.com", Age = 30 };
        var model = new Student { Id = "s4", Name = create.Name, Email = create.Email, Age = create.Age, UpdateDate = DateTime.UtcNow };
        _commandMock.Setup(c => c.Create(create)).ReturnsAsync(model);
        var ctrl = CreateController();

        var result = await ctrl.CreateStudent(create);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<Student>(ok.Value);
        Assert.Equal("s4", value.Id);
    }

    [Fact]
    public async Task CreateStudent_ReturnsBadRequest_OnInvalidAge()
    {
        var create = new CreateRequestStudent { Name = "Neo", Email = "neo@ex.com", Age = 0 };
        _commandMock.Setup(c => c.Create(create)).ThrowsAsync(new InvalidAge("bad"));
        var ctrl = CreateController();

        var result = await ctrl.CreateStudent(create);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateStudent_ReturnsOk()
    {
        var update = new UpdateRequestStudent { Name = "New" };
        var model = new Student { Id = "s5", Name = "New", Email = "e@e.com", Age = 18, UpdateDate = DateTime.UtcNow };
        _commandMock.Setup(c => c.Update("s5", update)).ReturnsAsync(model);
        var ctrl = CreateController();

        var result = await ctrl.UpdateStudent("s5", update);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<Student>(ok.Value);
        Assert.Equal("s5", value.Id);
    }

    [Fact]
    public async Task UpdateStudent_ReturnsBadRequest_OnInvalidAge()
    {
        var update = new UpdateRequestStudent { Age = 0 };
        _commandMock.Setup(c => c.Update("s5", update)).ThrowsAsync(new InvalidAge("bad"));
        var ctrl = CreateController();

        var result = await ctrl.UpdateStudent("s5", update);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateStudent_ReturnsNotFound_OnMissing()
    {
        var update = new UpdateRequestStudent { Name = "X" };
        _commandMock.Setup(c => c.Update("missing", update)).ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController();

        var result = await ctrl.UpdateStudent("missing", update);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteStudent_ReturnsOk()
    {
        var model = new Student { Id = "s6", Name = "Del", Email = "d@e.com", Age = 19, UpdateDate = DateTime.UtcNow };
        _commandMock.Setup(c => c.Delete("s6")).ReturnsAsync(model);
        var ctrl = CreateController();

        var result = await ctrl.DeleteStudent("s6");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteStudent_ReturnsNotFound()
    {
        _commandMock.Setup(c => c.Delete(It.IsAny<string>())).ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController();

        var result = await ctrl.DeleteStudent("missing");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateBookForStudent_ReturnsOk()
    {
        var req = new BookCreateDTO { Name = "Book1", Created_at = DateTime.UtcNow };
        var model = new Student { Id = "s7", Name = "B", Email = "b@e.com", Age = 20, UpdateDate = DateTime.UtcNow, StudentBooks = new List<Book>() };
        _commandMock.Setup(c => c.CreateBookForStudent("s7", req)).ReturnsAsync(model);
        var ctrl = CreateController();

        var result = await ctrl.CreateBookForStudent("s7", req);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateBookForStudent_ReturnsNotFound_OnMissingStudent()
    {
        var req = new BookCreateDTO { Name = "Book1", Created_at = DateTime.UtcNow };
        _commandMock.Setup(c => c.CreateBookForStudent(It.IsAny<string>(), req)).ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController();

        var result = await ctrl.CreateBookForStudent("missing", req);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateBookForStudent_ReturnsBadRequest_OnInvalidName()
    {
        var req = new BookCreateDTO { Name = "", Created_at = DateTime.UtcNow };
        _commandMock.Setup(c => c.CreateBookForStudent(It.IsAny<string>(), req)).ThrowsAsync(new InvalidName("bad"));
        var ctrl = CreateController();

        var result = await ctrl.CreateBookForStudent("s7", req);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateBookForStudent_ReturnsOk()
    {
        var req = new BookUpdateDTO { Name = "B2", Created_at = DateTime.UtcNow };
        var model = new Student { Id = "s8", Name = "U", Email = "u@e.com", Age = 22, UpdateDate = DateTime.UtcNow, StudentBooks = new List<Book>() };
        _commandMock.Setup(c => c.UpdateBookForStudent("s8", "b1", req)).ReturnsAsync(model);
        var ctrl = CreateController();

        var result = await ctrl.UpdateBookForStudent("s8", "b1", req);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateBookForStudent_ReturnsNotFound()
    {
        var req = new BookUpdateDTO { Name = "B2", Created_at = DateTime.UtcNow };
        _commandMock.Setup(c => c.UpdateBookForStudent(It.IsAny<string>(), It.IsAny<string>(), req))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController();

        var result = await ctrl.UpdateBookForStudent("s8", "missing", req);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateBookForStudent_ReturnsBadRequest_OnInvalidName()
    {
        var req = new BookUpdateDTO { Name = "", Created_at = DateTime.UtcNow };
        _commandMock.Setup(c => c.UpdateBookForStudent(It.IsAny<string>(), It.IsAny<string>(), req))
            .ThrowsAsync(new InvalidName("bad"));
        var ctrl = CreateController();

        var result = await ctrl.UpdateBookForStudent("s8", "b1", req);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteBookForStudent_ReturnsOk()
    {
        var model = new Student { Id = "s9", Name = "D", Email = "d@e.com", Age = 23, UpdateDate = DateTime.UtcNow };
        _commandMock.Setup(c => c.DeleteBookForStudent("s9", "b1")).ReturnsAsync(model);
        var ctrl = CreateController();

        var result = await ctrl.DeleteBookForStudent("s9", "b1");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteBookForStudent_ReturnsNotFound()
    {
        _commandMock.Setup(c => c.DeleteBookForStudent(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController();

        var result = await ctrl.DeleteBookForStudent("s9", "missing");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task EnrollmentCourse_ReturnsOk()
    {
        var course = new Course { Id = "c1", Name = "Math" };
        _courseQueryMock.Setup(c => c.GetByName("Math")).ReturnsAsync(course);
        var model = new Student { Id = "s10", Name = "E", Email = "e@e.com", Age = 26, UpdateDate = DateTime.UtcNow };
        _commandMock.Setup(c => c.EnrollmentCourse("s10", course)).ReturnsAsync(model);
        var ctrl = CreateController();

        var result = await ctrl.EnrollmentCourse("s10", "Math");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task EnrollmentCourse_ReturnsNotFound_WhenCourseMissing()
    {
        _courseQueryMock.Setup(c => c.GetByName(It.IsAny<string>())).ReturnsAsync((Course?)null);
        var ctrl = CreateController();

        var result = await ctrl.EnrollmentCourse("s10", "MissingCourse");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task EnrollmentCourse_ReturnsBadRequest_OnInvalidCourse()
    {
        var course = new Course { Id = "c1", Name = "Math" };
        _courseQueryMock.Setup(c => c.GetByName("Math")).ReturnsAsync(course);
        _commandMock.Setup(c => c.EnrollmentCourse("s10", course)).ThrowsAsync(new InvalidCourse("bad"));
        var ctrl = CreateController();

        var result = await ctrl.EnrollmentCourse("s10", "Math");

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UnEnrollmentCourse_ReturnsOk()
    {
        var course = new Course { Id = "c1", Name = "Math" };
        _courseQueryMock.Setup(c => c.GetByName("Math")).ReturnsAsync(course);
        var model = new Student { Id = "s11", Name = "U", Email = "u@e.com", Age = 27, UpdateDate = DateTime.UtcNow };
        _commandMock.Setup(c => c.UnEnrollmentCourse("s11", course)).ReturnsAsync(model);
        var ctrl = CreateController();

        var result = await ctrl.UnEnrollmentCourse("s11", "Math");

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task UnEnrollmentCourse_ReturnsNotFound_WhenCourseMissing()
    {
        _courseQueryMock.Setup(c => c.GetByName(It.IsAny<string>())).ReturnsAsync((Course?)null);
        var ctrl = CreateController();

        var result = await ctrl.UnEnrollmentCourse("s11", "Missing");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UnEnrollmentCourse_ReturnsBadRequest_OnInvalid()
    {
        var course = new Course { Id = "c1", Name = "Math" };
        _courseQueryMock.Setup(c => c.GetByName("Math")).ReturnsAsync(course);
        _commandMock.Setup(c => c.UnEnrollmentCourse("s11", course)).ThrowsAsync(new InvalidCourse("bad"));
        var ctrl = CreateController();

        var result = await ctrl.UnEnrollmentCourse("s11", "Math");

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UnEnrollmentCourse_ReturnsNotFound_OnMissingStudent()
    {
        var course = new Course { Id = "c1", Name = "Math" };
        _courseQueryMock.Setup(c => c.GetByName("Math")).ReturnsAsync(course);
        _commandMock.Setup(c => c.UnEnrollmentCourse(It.IsAny<string>(), It.IsAny<Course>())).ThrowsAsync(new ItemDoesNotExist("nf"));
        var ctrl = CreateController();

        var result = await ctrl.UnEnrollmentCourse("missing", "Math");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}