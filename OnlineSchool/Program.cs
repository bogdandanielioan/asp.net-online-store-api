using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Books.Repository;
using OnlineSchool.Books.Repository.interfaces;
using OnlineSchool.Books.Services;
using OnlineSchool.Books.Services.interfaces;
using OnlineSchool.Courses.Repository;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.Courses.Services;
using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.Data;
using OnlineSchool.Enrolments.Repository;
using OnlineSchool.Enrolments.Repository.interfaces;
using OnlineSchool.Enrolments.Services;
using OnlineSchool.Enrolments.Services.interfaces;
using OnlineSchool.StudentCards.Repository;
using OnlineSchool.StudentCards.Repository.interfaces;
using OnlineSchool.StudentCards.Services;
using OnlineSchool.StudentCards.Services.interfaces;
using OnlineSchool.Students.Repository;
using OnlineSchool.Students.Repository.interfaces;
using OnlineSchool.Students.Services;
using OnlineSchool.Students.Services.interfaces;
using OnlineSchool.StudentsCard.Repository;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IRepositoryStudentCard, RepositoryStudentCard>();
builder.Services.AddScoped<IQueryServiceStudentCard, QueryServiceStudentCards>();

builder.Services.AddScoped<IRepositoryStudent, RepositoryStudent>();
builder.Services.AddScoped<IQueryServiceStudent, QueryServiceStudents>();
builder.Services.AddScoped<ICommandServiceStudent, CommandServiceStudents>();

builder.Services.AddScoped<IRepositoryBook, RepositoryBook>();
builder.Services.AddScoped<IQueryServiceBook, QueryServiceBook>();

builder.Services.AddScoped<IRepositoryCourse, RepositoryCourse>();
builder.Services.AddScoped<IQueryServiceCourse, QueryServiceCourse>();
builder.Services.AddScoped<ICommandServiceCourse, CommandServiceCourse>();

builder.Services.AddScoped<IRepositoryEnrolment, RepositoryEnrolment>();
builder.Services.AddScoped<IQueryServiceEnrolment, QueryServiceEnrolment>();

builder.Services.AddDbContext<AppDbContext>(op => op.UseMySql(builder.Configuration.GetConnectionString("Default")!,
    new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb.AddMySql5().WithGlobalConnectionString(builder.Configuration.GetConnectionString("Default"))
    .ScanIn(typeof(Program).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("Default");
    EnsureDatabaseExists(connStr);

    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.Run();

static void EnsureDatabaseExists(string? connectionString)
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'Default' is not configured.");
    }

    var builder = new MySqlConnectionStringBuilder(connectionString);
    var database = builder.Database;
    if (string.IsNullOrWhiteSpace(database))
    {
        // Nothing to ensure if DB name is not present
        return;
    }

    // Build a server-level connection string (no database)
    builder.Database = string.Empty;

    using var connection = new MySqlConnection(builder.ConnectionString);
    connection.Open();

    using var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{database}`;", connection);
    cmd.ExecuteNonQuery();
}
