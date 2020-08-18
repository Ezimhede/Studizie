namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGroupTypesAndEntryTypesToGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "GroupTypesId", c => c.Int());
            AddColumn("dbo.Groups", "EntryTypesId", c => c.Int());
            CreateIndex("dbo.Groups", "GroupTypesId");
            CreateIndex("dbo.Groups", "EntryTypesId");
            AddForeignKey("dbo.Groups", "EntryTypesId", "dbo.EntryTypes", "Id");
            AddForeignKey("dbo.Groups", "GroupTypesId", "dbo.GroupTypes", "Id");
            DropColumn("dbo.Groups", "GroupType");
            DropColumn("dbo.Groups", "EntryType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "EntryType", c => c.String());
            AddColumn("dbo.Groups", "GroupType", c => c.String());
            DropForeignKey("dbo.Groups", "GroupTypesId", "dbo.GroupTypes");
            DropForeignKey("dbo.Groups", "EntryTypesId", "dbo.EntryTypes");
            DropIndex("dbo.Groups", new[] { "EntryTypesId" });
            DropIndex("dbo.Groups", new[] { "GroupTypesId" });
            DropColumn("dbo.Groups", "EntryTypesId");
            DropColumn("dbo.Groups", "GroupTypesId");
        }
    }
}
