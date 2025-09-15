using FluentMigrator;

namespace OnlineSchool.Data.Migrations
{
    [Migration(1)]
    public class CreateDatabaseIfNotExists : Migration
    {
        public override void Up()
        {
            Execute.Sql("CREATE DATABASE IF NOT EXISTS `online_school_db_api`;");
        }

        public override void Down()
        {
            
        }
    }
}