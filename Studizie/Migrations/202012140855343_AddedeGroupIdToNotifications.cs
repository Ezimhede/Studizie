namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedeGroupIdToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "GroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "GroupId");
        }
    }
}
