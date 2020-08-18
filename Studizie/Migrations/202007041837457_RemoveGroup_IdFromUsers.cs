namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGroup_IdFromUsers : DbMigration
    {
        public override void Up()
        {
            DropIndex("AspNetUsers", "IX_Group_Id");
            DropForeignKey("AspNetUsers", "FK_dbo.AspNetUsers_dbo.Groups_Group_Id");
            DropColumn("dbo.AspNetUsers", "Group_Id");
        }
        
        public override void Down()
        {
        }
    }
}
