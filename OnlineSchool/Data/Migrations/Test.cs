using FluentMigrator;

namespace OnlineSchool.Data.Migrations
{
    [Migration(214032024)]
    public class Test : Migration
    {
        public override void Up()
        {
            Execute.Script(@"./Data/Scripts/data.sql");
        }

        public override void Down()
        {

        }

    }
}
