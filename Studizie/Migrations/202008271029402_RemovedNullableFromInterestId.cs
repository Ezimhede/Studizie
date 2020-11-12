namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedNullableFromInterestId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "InterestsId", "dbo.Interests");
            DropIndex("dbo.Groups", new[] { "InterestsId" });
            AlterColumn("dbo.Groups", "InterestsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "InterestsId");
            AddForeignKey("dbo.Groups", "InterestsId", "dbo.Interests", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "InterestsId", "dbo.Interests");
            DropIndex("dbo.Groups", new[] { "InterestsId" });
            AlterColumn("dbo.Groups", "InterestsId", c => c.Int());
            CreateIndex("dbo.Groups", "InterestsId");
            AddForeignKey("dbo.Groups", "InterestsId", "dbo.Interests", "Id");
        }
    }
}
