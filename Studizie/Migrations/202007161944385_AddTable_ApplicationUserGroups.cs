namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_ApplicationUserGroups : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.ApplicationUserGroups", newName: "GroupApplicationUsers");
            //DropPrimaryKey("dbo.GroupApplicationUsers");
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.GroupId);
            
            //AddPrimaryKey("dbo.GroupApplicationUsers", new[] { "Group_Id", "ApplicationUser_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.ApplicationUserGroups", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "ApplicationUserId" });
            DropPrimaryKey("dbo.GroupApplicationUsers");
            DropTable("dbo.ApplicationUserGroups");
            AddPrimaryKey("dbo.GroupApplicationUsers", new[] { "ApplicationUser_Id", "Group_Id" });
            RenameTable(name: "dbo.GroupApplicationUsers", newName: "ApplicationUserGroups");
        }
    }
}
