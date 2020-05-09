namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Groups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        About = c.String(nullable: false),
                        NumberOfMembers = c.Int(nullable: false),
                        GroupImage = c.String(),
                        MeetingPoint = c.String(),
                        TimeOfMeeting = c.String(),
                        GroupCreator = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        Surname = c.String(),
                        Email = c.String(),
                        InterestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interests", t => t.InterestId, cascadeDelete: true)
                .Index(t => t.InterestId);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "MemberId", "dbo.Members");
            DropForeignKey("dbo.Members", "InterestId", "dbo.Interests");
            DropIndex("dbo.Members", new[] { "InterestId" });
            DropIndex("dbo.Groups", new[] { "MemberId" });
            DropTable("dbo.Interests");
            DropTable("dbo.Members");
            DropTable("dbo.Groups");
        }
    }
}
