using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Data;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Mappings;
using OnlineSchool.Students.Repository;

namespace Teste.Unit;

public class StudentsRolesUnitTests
{
    [Fact]
    public async Task Repository_Create_AssignsUserRole()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"StudentsRoles_{Guid.NewGuid()}")
            .Options;

        using var ctx = new AppDbContext(options);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfilesStudent>();
        });
        var mapper = mapperConfig.CreateMapper();

        var repo = new RepositoryStudent(ctx, mapper);

        var req = new CreateRequestStudent { Name = "Role User", Email = "r@u.com", Age = 18 };
        var created = await repo.Create(req);

        Assert.NotNull(created);
        Assert.Equal("User", created.Role);
    }
}
