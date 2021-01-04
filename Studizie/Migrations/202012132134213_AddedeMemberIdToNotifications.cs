namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedeMemberIdToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "MemberId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "MemberId");
        }
    }
}
