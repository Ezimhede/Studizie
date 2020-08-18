namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedColumn_DateCreated_FromApplicationUserGroups : DbMigration
    {
        public override void Up()
        {
            DropColumn("ApplicationUserGroups", "DateCreated");
        }
        
        public override void Down()
        {
        }
    }
}
