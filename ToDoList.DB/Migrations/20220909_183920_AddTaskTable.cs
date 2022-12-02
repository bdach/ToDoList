using FluentMigrator;

namespace ToDoList.DB.Migrations;

[Migration(2022_09_09__18_39_20)]
public class AddTaskTable : Migration
{
    public override void Up()
    {
        Create.Table("Tasks")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Title").AsString()
            .WithColumn("Done").AsBoolean();
    }

    public override void Down()
    {
        Delete.Table("Tasks");
    }
}