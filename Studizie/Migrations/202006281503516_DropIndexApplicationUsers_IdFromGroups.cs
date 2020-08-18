namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropIndexApplicationUsers_IdFromGroups : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Groups", new[] { "ApplicationUsers_Id" });
        }
        
        public override void Down()
        {
        }
    }
}
