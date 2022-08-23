using System;
using FluentMigrator;

namespace ContractControlCentre.DB.Migrations
{
    [Migration(3)]
    public class EmployeeMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("Employee_DB");
        }


        public override void Up()
        {
            Create.Table("Employee_DB")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsTime();
        }
    }
}

