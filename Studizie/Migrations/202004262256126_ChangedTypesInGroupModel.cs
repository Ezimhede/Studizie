namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTypesInGroupModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "GroupType", c => c.String());
            AddColumn("dbo.Groups", "EntryType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "EntryType");
            DropColumn("dbo.Groups", "GroupType");
        }
    }
}
