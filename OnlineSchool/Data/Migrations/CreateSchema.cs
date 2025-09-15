using FluentMigrator;
using OnlineSchool.Students.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineSchool.Data.Migrations
{
    [Migration(13442)]
    public class CreateSchema : Migration
    {
        public override void Up()
        {

            Create.Table("students")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Age").AsInt32().NotNullable()
                .WithColumn("UpdateDate").AsDateTime().Nullable();


            Create.Table("books")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("IdStudent").AsInt32().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Created").AsDate().NotNullable();


            Create.Table("studentscard")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("IdStudent").AsInt32().NotNullable()
                .WithColumn("Namecard").AsString().NotNullable();


            Create.Table("courses")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Department").AsString().NotNullable();


            Create.Table("enrolments")
              .WithColumn("Id").AsInt32().PrimaryKey().Identity()
              .WithColumn("IdCourse").AsInt32().NotNullable()
              .WithColumn("IdStudent").AsInt32().NotNullable()
              .WithColumn("Created").AsDateTime().NotNullable();
        }


        public override void Down()
        {

        }


    }

}
