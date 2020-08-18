namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameApplicationUsers_IdInGroups : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "ApplicationUsers_Id_Id", newName: "ApplicationUsers_Id1");
            RenameIndex(table: "dbo.Groups", name: "IX_ApplicationUsers_Id_Id", newName: "IX_ApplicationUsers_Id1");
            AddColumn("dbo.Groups", "ApplicationUsers_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "ApplicationUsersId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Groups", name: "IX_ApplicationUsers_Id1", newName: "IX_ApplicationUsers_Id_Id");
            RenameColumn(table: "dbo.Groups", name: "ApplicationUsers_Id1", newName: "ApplicationUsers_Id_Id");
        }
    }
}
