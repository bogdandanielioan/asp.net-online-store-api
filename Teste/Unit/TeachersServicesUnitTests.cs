using Moq;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Teachers.Repository.interfaces;
using OnlineSchool.Teachers.Services;
using Xunit;

namespace Teste.Unit;

public class TeachersServicesUnitTests
{
    [Fact]
    public async Task Command_Create_AssignsIdDateRole_AndCallsRepo()
    {
        var repo = new Mock<IRepositoryTeacher>();
        repo.Setup(r => r.CreateAsync(It.IsAny<Teacher>()))
            .ReturnsAsync((Teacher t) => t);
        var svc = new CommandServiceTeachers(repo.Object);

        var teacher = new Teacher { Name = "John", Email = "john@example.com" };
        var created = await svc.CreateAsync(teacher);

        Assert.False(string.IsNullOrWhiteSpace(created.Id));
        Assert.True(created.UpdateDate != default);
        Assert.Equal("Admin", created.Role);
        repo.Verify(r => r.CreateAsync(It.IsAny<Teacher>()), Times.Once);
    }

    [Fact]
    public async Task Command_Create_EmptyName_Throws()
    {
        var repo = new Mock<IRepositoryTeacher>();
        var svc = new CommandServiceTeachers(repo.Object);
        await Assert.ThrowsAsync<InvalidName>(() => svc.CreateAsync(new Teacher { Name = "", Email = "x@y" }));
    }

    [Fact]
    public async Task Command_Update_NotFound_Throws()
    {
        var repo = new Mock<IRepositoryTeacher>();
        repo.Setup(r => r.GetByIdAsync("missing")).ReturnsAsync((Teacher?)null);
        var svc = new CommandServiceTeachers(repo.Object);

        await Assert.ThrowsAsync<ItemDoesNotExist>(() => svc.UpdateAsync("missing", new Teacher { Name = "A", Email = "a@b" }));
    }

    [Fact]
    public async Task Command_Update_Valid_CallsRepoAndReturnsUpdated()
    {
        var repo = new Mock<IRepositoryTeacher>();
        var existing = new Teacher { Id = "t1", Name = "Old", Email = "o@e" };
        repo.Setup(r => r.GetByIdAsync("t1")).ReturnsAsync(existing);
        repo.Setup(r => r.UpdateAsync("t1", It.IsAny<Teacher>()))
            .ReturnsAsync((string id, Teacher t) => t);
        var svc = new CommandServiceTeachers(repo.Object);

        var updated = await svc.UpdateAsync("t1", new Teacher { Name = "New", Email = "o@e", Subject = "Math" });
        Assert.Equal("New", updated.Name);
        Assert.Equal("Math", updated.Subject);
        repo.Verify(r => r.UpdateAsync("t1", It.IsAny<Teacher>()), Times.Once);
    }

    [Fact]
    public async Task Command_Delete_NotFound_Throws()
    {
        var repo = new Mock<IRepositoryTeacher>();
        repo.Setup(r => r.DeleteAsync("x")).ReturnsAsync((Teacher?)null);
        var svc = new CommandServiceTeachers(repo.Object);

        await Assert.ThrowsAsync<ItemDoesNotExist>(() => svc.DeleteAsync("x"));
    }

    [Fact]
    public async Task Command_Delete_ReturnsDeleted()
    {
        var repo = new Mock<IRepositoryTeacher>();
        repo.Setup(r => r.DeleteAsync("t1")).ReturnsAsync(new Teacher { Id = "t1" });
        var svc = new CommandServiceTeachers(repo.Object);

        var del = await svc.DeleteAsync("t1");
        Assert.Equal("t1", del.Id);
    }

    [Fact]
    public async Task Query_GetAll_Empty_Throws()
    {
        var repo = new Mock<IRepositoryTeacher>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Teacher>());
        var svc = new QueryServiceTeachers(repo.Object);

        await Assert.ThrowsAsync<ItemsDoNotExist>(() => svc.GetAllAsync());
    }

    [Fact]
    public async Task Query_GetAll_Returns()
    {
        var repo = new Mock<IRepositoryTeacher>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Teacher> { new Teacher { Id = "t1", Name = "N", Email = "e@e" } });
        var svc = new QueryServiceTeachers(repo.Object);

        var list = await svc.GetAllAsync();
        Assert.Single(list);
    }

    [Fact]
    public async Task Query_GetById_NotFound_Throws()
    {
        var repo = new Mock<IRepositoryTeacher>();
        repo.Setup(r => r.GetByIdAsync("x")).ReturnsAsync((Teacher?)null);
        var svc = new QueryServiceTeachers(repo.Object);
        await Assert.ThrowsAsync<ItemDoesNotExist>(() => svc.GetByIdAsync("x"));
    }

    [Fact]
    public async Task Query_GetByEmail_NotFound_Throws()
    {
        var repo = new Mock<IRepositoryTeacher>();
        repo.Setup(r => r.GetByEmailAsync("x@x")).ReturnsAsync((Teacher?)null);
        var svc = new QueryServiceTeachers(repo.Object);
        await Assert.ThrowsAsync<ItemDoesNotExist>(() => svc.GetByEmailAsync("x@x"));
    }
}