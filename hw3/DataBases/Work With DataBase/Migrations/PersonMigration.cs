using FluentMigrator;

namespace ContractControlCentre.DB.Migrations
{
    [Migration(5)]
    public class PersonMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("Person_DB");
        }


        public override void Up()
        {
            Create.Table("Person_DB")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString()
                .WithColumn("LastName").AsString()
                .WithColumn("Email").AsString()
                .WithColumn("Company").AsString()
                .WithColumn("Age").AsInt32();
        }
    }
}

