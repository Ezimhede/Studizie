namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateEntryTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO EntryTypes (Name) VALUES ('Free')");
            Sql("INSERT INTO EntryTypes (Name) VALUES ('Paid')");
        }
        
        public override void Down()
        {
        }
    }
}
