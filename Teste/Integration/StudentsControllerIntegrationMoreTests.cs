using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using Xunit;

namespace Teste.Integration;

public class StudentsControllerIntegrationMoreTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public StudentsControllerIntegrationMoreTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetStudentCard_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudent/studentCard?id=stu1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var card = await resp.Content.ReadFromJsonAsync<OnlineSchool.StudentCards.Models.StudentCard>(JsonOpts);
        Assert.NotNull(card);
        Assert.Equal("AliceCard", card!.Namecard);
    }

    [Fact]
    public async Task Create_Update_Delete_Student_Flow()
    {
        // Create
        var create = new CreateRequestStudent { Name = "Bob", Email = "bob@example.com", Age = 21 };
        var cResp = await _client.PostAsJsonAsync("/api/v1/ControllerStudent/createStudent", create);
        Assert.Equal(HttpStatusCode.OK, cResp.StatusCode);
        var created = await cResp.Content.ReadFromJsonAsync<Student>(JsonOpts);
        Assert.NotNull(created);
        Assert.False(string.IsNullOrWhiteSpace(created!.Id));

        // Update valid
        var update = new UpdateRequestStudent { Name = "Bobby" };
        var uResp = await _client.PutAsJsonAsync($"/api/v1/ControllerStudent/updateStudent?id={created.Id}", update);
        Assert.Equal(HttpStatusCode.OK, uResp.StatusCode);
        var updated = await uResp.Content.ReadFromJsonAsync<Student>(JsonOpts);
        Assert.NotNull(updated);
        Assert.Equal("Bobby", updated!.Name);

        // Update invalid age
        var bad = new UpdateRequestStudent { Age = 0 };
        var badResp = await _client.PutAsJsonAsync($"/api/v1/ControllerStudent/updateStudent?id={created.Id}", bad);
        Assert.Equal(HttpStatusCode.BadRequest, badResp.StatusCode);

        // Delete
        var dResp = await _client.DeleteAsync($"/api/v1/ControllerStudent/deleteStudent?id={created.Id}");
        Assert.Equal(HttpStatusCode.OK, dResp.StatusCode);
        var dResp2 = await _client.DeleteAsync($"/api/v1/ControllerStudent/deleteStudent?id={created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, dResp2.StatusCode);
    }

    [Fact]
    public async Task Book_CRUD_ForStudent()
    {
        // Create a new book for existing student stu1
        var bCreate = new BookCreateDTO { Name = "Geometry", Created_at = DateTime.UtcNow };
        var cResp = await _client.PostAsJsonAsync($"/api/v1/ControllerStudent/createBookForStudent?idStudent=stu1", bCreate);
        Assert.Equal(HttpStatusCode.OK, cResp.StatusCode);

        // Update existing seeded book b1
        var bUpdate = new BookUpdateDTO { Name = "Algebra Advanced", Created_at = DateTime.UtcNow };
        var uResp = await _client.PutAsJsonAsync($"/api/v1/ControllerStudent/updateBookForStudent?idStudent=stu1&idBook=b1", bUpdate);
        Assert.Equal(HttpStatusCode.OK, uResp.StatusCode);

        // Delete existing seeded book b1
        var dResp = await _client.DeleteAsync($"/api/v1/ControllerStudent/deleteBookForStudent?idStudent=stu1&idBook=b1");
        Assert.Equal(HttpStatusCode.OK, dResp.StatusCode);
    }

    [Fact]
    public async Task Enrollment_Flow_Duplicate_And_MissingCourse()
    {
        // Duplicate enrolment should be BadRequest (Alice already enrolled to Math)
        var dup = await _client.PostAsync($"/api/v1/ControllerStudent/enrollmentCourse?idStudent=stu1&name=Math", content: null);
        Assert.Equal(HttpStatusCode.BadRequest, dup.StatusCode);

        // Missing course should be NotFound
        var missing = await _client.PostAsync($"/api/v1/ControllerStudent/enrollmentCourse?idStudent=stu1&name=MissingCourse", content: null);
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    [Fact]
    public async Task Enrollment_And_Unenrollment_NewCourse_Succeeds()
    {
        // Create a new course
        var courseReq = new CreateRequestCourse { Name = "History", Department = "Arts", TeacherId = "t1" };
        var cResp = await _client.PostAsJsonAsync("/api/v1/ControllerCourse/createCourse", courseReq);
        Assert.Equal(HttpStatusCode.OK, cResp.StatusCode);
        var course = await cResp.Content.ReadFromJsonAsync<Course>(JsonOpts);
        Assert.NotNull(course);

        // Enroll
        var enroll = await _client.PostAsync($"/api/v1/ControllerStudent/enrollmentCourse?idStudent=stu1&name={Uri.EscapeDataString(course!.Name)}", null);
        Assert.Equal(HttpStatusCode.OK, enroll.StatusCode);

        // Unenroll
        var unenroll = await _client.DeleteAsync($"/api/v1/ControllerStudent/unenrollmentCourse?idStudent=stu1&name={Uri.EscapeDataString(course!.Name)}");
        Assert.Equal(HttpStatusCode.OK, unenroll.StatusCode);
    }
}
