using FluentMigrator;

namespace OnlineSchool.Data.Migrations
{
    // This migration is intentionally idempotent and safe to run multiple times.
    // It will no-op if the database already exists.
    [Migration(1)]
    public class CreateDatabaseIfNotExists : Migration
    {
        public override void Up()
        {
            // NOTE: Normally, creating the database should happen before the migration runner connects.
            // Program.cs already ensures DB creation at startup. This migration is provided per requirement
            // and will be a no-op once connected to the target DB. The DB name matches appsettings.json.
            Execute.Sql("CREATE DATABASE IF NOT EXISTS `online_school_db_api`;");
        }

        public override void Down()
        {
            // Do not drop the database in Down() to avoid destructive operations.
        }
    }
}