using FluentMigrator;

namespace Ticketron.DB.Migrations;

[Migration(2022_09_09__18_44_39)]
public class AddTaskGroupTable : Migration
{
    public override void Up()
    {
        Create.Table("TaskGroups")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Icon").AsString()
            .WithColumn("Name").AsString();

        Alter.Table("Tasks")
            .AddColumn("GroupId").AsInt32().ForeignKey("FK_Task_TaskGroup", "TaskGroups", "Id");
    }

    public override void Down()
    {
        Delete.Column("TaskGroupId")
            .FromTable("Tasks");

        Delete.Table("TaskGroups");
    }
}