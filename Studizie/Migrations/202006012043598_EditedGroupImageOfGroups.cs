namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedGroupImageOfGroups : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "GroupImage", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "GroupImage", c => c.String());
        }
    }
}
