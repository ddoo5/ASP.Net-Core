using System;
using FluentMigrator;

namespace ContractControlCentre.DB.Migrations
{
    [Migration(2)]
    public class CustomerMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("Customer_DB");
        }


        public override void Up()
        {
            Create.Table("Customer_DB")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsTime();
        }
    }
}

