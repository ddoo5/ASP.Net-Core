using System;
using FluentMigrator;

namespace ContractControlCentre.DB.Migrations
{
    [Migration(4)]
    public class InvoiceMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("Invoice_DB");
        }


        public override void Up()
        {
            Create.Table("Invoice_DB")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsTime();
        }
    }
}

