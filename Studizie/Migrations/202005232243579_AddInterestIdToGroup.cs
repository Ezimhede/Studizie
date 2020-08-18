namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInterestIdToGroup : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "Interests_Id", newName: "InterestsId");
            RenameIndex(table: "dbo.Groups", name: "IX_Interests_Id", newName: "IX_InterestsId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Groups", name: "IX_InterestsId", newName: "IX_Interests_Id");
            RenameColumn(table: "dbo.Groups", name: "InterestsId", newName: "Interests_Id");
        }
    }
}
