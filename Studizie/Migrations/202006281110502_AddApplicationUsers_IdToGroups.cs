namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationUsers_IdToGroups : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "ApplicationUsers_Id", newName: "ApplicationUsers_Id_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_ApplicationUsers_Id", newName: "IX_ApplicationUsers_Id_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Groups", name: "IX_ApplicationUsers_Id_Id", newName: "IX_ApplicationUsers_Id");
            RenameColumn(table: "dbo.Groups", name: "ApplicationUsers_Id_Id", newName: "ApplicationUsers_Id");
        }
    }
}
