namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveApplicationUserFromGroups : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Groups", "ApplicationUsersId");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "ApplicationUsersId");
        }
    }
}
