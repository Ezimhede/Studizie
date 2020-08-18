namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedGroupTypesAndEntryTypesOfGroups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "EntryTypesId", "dbo.EntryTypes");
            DropForeignKey("dbo.Groups", "GroupTypesId", "dbo.GroupTypes");
            DropIndex("dbo.Groups", new[] { "GroupTypesId" });
            DropIndex("dbo.Groups", new[] { "EntryTypesId" });
            AlterColumn("dbo.Groups", "GroupTypesId", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "EntryTypesId", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "GroupTypesId");
            CreateIndex("dbo.Groups", "EntryTypesId");
            AddForeignKey("dbo.Groups", "EntryTypesId", "dbo.EntryTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Groups", "GroupTypesId", "dbo.GroupTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "GroupTypesId", "dbo.GroupTypes");
            DropForeignKey("dbo.Groups", "EntryTypesId", "dbo.EntryTypes");
            DropIndex("dbo.Groups", new[] { "EntryTypesId" });
            DropIndex("dbo.Groups", new[] { "GroupTypesId" });
            AlterColumn("dbo.Groups", "EntryTypesId", c => c.Int());
            AlterColumn("dbo.Groups", "GroupTypesId", c => c.Int());
            CreateIndex("dbo.Groups", "EntryTypesId");
            CreateIndex("dbo.Groups", "GroupTypesId");
            AddForeignKey("dbo.Groups", "GroupTypesId", "dbo.GroupTypes", "Id");
            AddForeignKey("dbo.Groups", "EntryTypesId", "dbo.EntryTypes", "Id");
        }
    }
}
