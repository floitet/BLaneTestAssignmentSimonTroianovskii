using System.Reflection;
using BallastLaneTestAssignment.Domain.Enums;
using FluentMigrator;

namespace Sql.Migrations;

[Migration(202310151028)]
public class InitialMigration : Migration
{
     public override void Up()
    {
        Create
            .Table("PrescriptionList")
            .WithColumn("ListId").AsInt32().PrimaryKey().Identity()
            .WithColumn("Title").AsAnsiString(int.MaxValue)
            .WithColumn("ColourCode").AsAnsiString(int.MaxValue)
            .WithColumn("CreatedAt").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime).Nullable()
            .WithColumn("LastModifiedAt").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime).Nullable()
            .WithColumn("CreatedBy").AsAnsiString(int.MaxValue).NotNullable()
            .WithColumn("LastModifiedBy").AsAnsiString(int.MaxValue).NotNullable();

        Create
            .Table("PrescriptionItem")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("ListId").AsInt32().NotNullable()
            .WithColumn("Title").AsAnsiString(int.MaxValue).Nullable()
            .WithColumn("Note").AsAnsiString(int.MaxValue).Nullable()
            .WithColumn("Priority").AsInt32().WithDefaultValue((int)PriorityLevel.Medium)
            .WithColumn("Reminder").AsDateTime().Nullable()
            .WithColumn("Done").AsBoolean()
            .WithColumn("CreatedAt").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime).Nullable()
            .WithColumn("LastModifiedAt").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime).Nullable()
            .WithColumn("CreatedBy").AsAnsiString(int.MaxValue).NotNullable()
            .WithColumn("LastModifiedBy").AsAnsiString(int.MaxValue).NotNullable();

        Create
            .ForeignKey("FK_PrescriptionItem_PrescriptionList")
            .FromTable("PrescriptionItem").InSchema(null)
            .ForeignColumn("ListId")
            .ToTable("PrescriptionList").InSchema(null)
            .PrimaryColumn("ListId");
    }

    public override void Down()
    {
        Delete.Table("PrescriptionItem");
        Delete.Table("PrescriptionList");
    }
}