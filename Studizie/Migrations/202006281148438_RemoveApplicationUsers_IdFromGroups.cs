namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveApplicationUsers_IdFromGroups : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Groups", new[] { "ApplicationUsers_Id1" });
            DropColumn("dbo.Groups", "ApplicationUsers_Id");
            RenameColumn(table: "dbo.Groups", name: "ApplicationUsers_Id1", newName: "ApplicationUsers_Id");
            AlterColumn("dbo.Groups", "ApplicationUsers_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Groups", "ApplicationUsers_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Groups", new[] { "ApplicationUsers_Id" });
            AlterColumn("dbo.Groups", "ApplicationUsers_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Groups", name: "ApplicationUsers_Id", newName: "ApplicationUsers_Id1");
            AddColumn("dbo.Groups", "ApplicationUsers_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "ApplicationUsers_Id1");
        }
    }
}
