namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateToApplicationUserGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUserGroups", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUserGroups", "DateCreated");
        }
    }
}
