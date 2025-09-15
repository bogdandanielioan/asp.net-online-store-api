using FluentMigrator;

namespace OnlineSchool.Data.Migrations
{
    [Migration(13442)]
    public class CreateSchema : Migration
    {
        public override void Up()
        {
            // Create tables using a single string Id as primary key. No PublicId columns.
            Create.Table("students")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Age").AsInt32().NotNullable()
                .WithColumn("UpdateDate").AsDateTime().Nullable();

            Create.Table("books")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("IdStudent").AsString().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Created").AsDate().NotNullable();

            Create.Table("studentscard")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("IdStudent").AsString().NotNullable()
                .WithColumn("Namecard").AsString().NotNullable();

            Create.Table("courses")
                .WithColumn("Id").AsString().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Department").AsString().NotNullable();

            Create.Table("enrolments")
              .WithColumn("Id").AsString().PrimaryKey().NotNullable()
              .WithColumn("IdCourse").AsString().NotNullable()
              .WithColumn("IdStudent").AsString().NotNullable()
              .WithColumn("Created").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            // Intentionally left empty for now; rollback not defined.
        }
    }
}
