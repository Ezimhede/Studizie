namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteApplicationUsers_IdFromGroups : DbMigration
    {
        public override void Up()
        {
            DropColumn("Groups", "ApplicationUsers_Id");
        }
        
        public override void Down()
        {
        }
    }
}
