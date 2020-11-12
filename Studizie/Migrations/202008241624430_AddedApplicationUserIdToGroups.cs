namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedApplicationUserIdToGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Groups", "ApplicationUserId");
            AddForeignKey("dbo.Groups", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Groups", new[] { "ApplicationUserId" });
            DropColumn("dbo.Groups", "ApplicationUserId");
        }
    }
}
