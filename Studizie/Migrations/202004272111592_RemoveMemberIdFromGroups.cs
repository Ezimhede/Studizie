namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMemberIdFromGroups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "MemberId", "dbo.Members");
            DropIndex("dbo.Groups", new[] { "MemberId" });
            RenameColumn(table: "dbo.Groups", name: "MemberId", newName: "Member_Id");
            AlterColumn("dbo.Groups", "Member_Id", c => c.Int());
            CreateIndex("dbo.Groups", "Member_Id");
            AddForeignKey("dbo.Groups", "Member_Id", "dbo.Members", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Member_Id", "dbo.Members");
            DropIndex("dbo.Groups", new[] { "Member_Id" });
            AlterColumn("dbo.Groups", "Member_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Groups", name: "Member_Id", newName: "MemberId");
            CreateIndex("dbo.Groups", "MemberId");
            AddForeignKey("dbo.Groups", "MemberId", "dbo.Members", "Id", cascadeDelete: true);
        }
    }
}
