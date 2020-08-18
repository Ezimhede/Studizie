namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveApplicationUsersIdFromGroups : DbMigration
    {
        public override void Up()
        {
            DropIndex("AspNetUsers", "IX_Group_Id");
            DropForeignKey("AspNetUsers", "FK_dbo.AspNetUsers_dbo.Groups_Group_Id");
            DropColumn("dbo.Groups", "ApplicationUsersId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "ApplicationUsersId", c => c.String());
        }
    }
}
