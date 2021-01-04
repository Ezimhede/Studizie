namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNotificationTypeToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "NotificationType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "NotificationType");
        }
    }
}
