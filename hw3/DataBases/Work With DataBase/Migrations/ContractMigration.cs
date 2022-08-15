using FluentMigrator;

namespace ContractControlCentre.DB.Migrations
{
    [Migration(1)]
    public class ContractMigration : Migration
	{
        public override void Down()
        {
            Delete.Table("Contract_DB");
        }


        public override void Up()
        {
            Create.Table("Contract_DB")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsTime();
        }
    }
}

