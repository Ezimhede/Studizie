namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGroupsFromApplicationUsers : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "GroupsId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "GroupsId", c => c.Int());
        }
    }
}
