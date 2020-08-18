namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedApplicationUserToGroups : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "ApplicationUser_Id", newName: "ApplicationUsers_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUsers_Id");
            AddColumn("dbo.Groups", "ApplicationUsersId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "ApplicationUsersId");
            RenameIndex(table: "dbo.Groups", name: "IX_ApplicationUsers_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Groups", name: "ApplicationUsers_Id", newName: "ApplicationUser_Id");
        }
    }
}
