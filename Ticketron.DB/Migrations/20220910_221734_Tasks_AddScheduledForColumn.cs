using FluentMigrator;

namespace Ticketron.DB.Migrations;

[Migration(2022_09_10__22_17_34)]
public class Tasks_AddScheduledForColumn : Migration
{
    public override void Up()
    {
        Create.Column("ScheduledFor")
            .OnTable("Tasks")
            .AsDate().Nullable();
    }

    public override void Down()
    {
        Delete.Column("ScheduledFor")
            .FromTable("Tasks");
    }
}