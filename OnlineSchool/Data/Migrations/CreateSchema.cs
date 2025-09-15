using FluentMigrator;

namespace OnlineSchool.Data.Migrations
{
    [Migration(13442)]
    public class CreateSchema : Migration
    {
        public override void Up()
        {
            Create.Table("teachers")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Subject").AsString().Nullable()
                .WithColumn("UpdateDate").AsDateTime().NotNullable()
                .WithColumn("Role").AsString().Nullable()
                .WithColumn("PasswordHash").AsString().Nullable()
                .WithColumn("PasswordSalt").AsString().Nullable();

            Create.Table("students")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Age").AsInt32().NotNullable()
                .WithColumn("UpdateDate").AsDateTime().Nullable()
                .WithColumn("Role").AsString().Nullable()
                .WithColumn("PasswordHash").AsString().Nullable()
                .WithColumn("PasswordSalt").AsString().Nullable();

            Create.Table("books")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("IdStudent").AsString().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Created").AsDateTime().NotNullable();

            Create.Table("studentscard")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("IdStudent").AsString().NotNullable()
                .WithColumn("Namecard").AsString().NotNullable();

            Create.Table("courses")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Department").AsString().NotNullable()
                .WithColumn("TeacherId").AsString().Nullable();

        
            const string fkName = "fk_courses_teacherid";
            Create.ForeignKey(fkName)
                .FromTable("courses").ForeignColumn("TeacherId")
                .ToTable("teachers").PrimaryColumn("Id");

            Create.Table("enrolments")
              .WithColumn("Id").AsString().PrimaryKey().NotNullable()
              .WithColumn("IdCourse").AsString().NotNullable()
              .WithColumn("IdStudent").AsString().NotNullable()
              .WithColumn("Created").AsDateTime().NotNullable();
        }

        public override void Down()
        {
          
        }
    }
}
