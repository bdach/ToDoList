using FluentMigrator;

namespace Ticketron.DB.Migrations;

[Migration(2022_09_11__14_02_31)]
public class AddTaskLogEntriesTable : Migration
{
    public override void Up()
    {
        Create.Table("TaskLogEntries")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("TaskId").AsInt64().ForeignKey("FK_Task_TaskLogEntry", "Tasks", "Id")
            .WithColumn("Start").AsDateTime()
            .WithColumn("End").AsDateTime().Nullable()
            .WithColumn("Notes").AsString();
    }

    public override void Down()
    {
        Delete.Table("TaskLogEntries");
    }
}