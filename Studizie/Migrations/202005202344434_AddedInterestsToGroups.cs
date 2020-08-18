namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInterestsToGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Interests_Id", c => c.Int());
            CreateIndex("dbo.Groups", "Interests_Id");
            AddForeignKey("dbo.Groups", "Interests_Id", "dbo.Interests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Interests_Id", "dbo.Interests");
            DropIndex("dbo.Groups", new[] { "Interests_Id" });
            DropColumn("dbo.Groups", "Interests_Id");
        }
    }
}
