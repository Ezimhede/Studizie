namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropTable_ApplicationUserGroups : DbMigration
    {
        public override void Up()
        {
            DropTable("ApplicationUserGroups");
        }
        
        public override void Down()
        {
        }
    }
}
