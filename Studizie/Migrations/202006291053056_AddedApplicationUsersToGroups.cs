namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedApplicationUsersToGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "ApplicationUsersId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Groups", "ApplicationUsersId");
            AddForeignKey("dbo.Groups", "ApplicationUsersId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "ApplicationUsersId", "dbo.AspNetUsers");
            DropIndex("dbo.Groups", new[] { "ApplicationUsersId" });
            DropColumn("dbo.Groups", "ApplicationUsersId");
        }
    }
}
