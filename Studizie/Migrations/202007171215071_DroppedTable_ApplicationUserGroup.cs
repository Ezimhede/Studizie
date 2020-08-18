namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedTable_ApplicationUserGroup : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.GroupApplicationUsers", newName: "ApplicationUserGroups");
            DropForeignKey("dbo.ApplicationUserGroups", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.Groups");
            DropIndex("dbo.ApplicationUserGroups", new[] { "ApplicationUserId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropPrimaryKey("dbo.ApplicationUserGroups");
            //AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "ApplicationUser_Id", "Group_Id" });
            DropTable("dbo.ApplicationUserGroups");
        }
        
        public override void Down()
        {
           
        }
    }
}
