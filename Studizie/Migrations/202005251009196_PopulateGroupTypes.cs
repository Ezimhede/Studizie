namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateGroupTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO GroupTypes (Name) VALUES ('Open')");
            Sql("INSERT INTO GroupTypes (Name) VALUES ('Closed')");
        }
        
        public override void Down()
        {
        }
    }
}
