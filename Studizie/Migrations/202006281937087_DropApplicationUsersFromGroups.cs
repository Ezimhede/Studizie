namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropApplicationUsersFromGroups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Groups", "FK_dbo.Groups_dbo.AspNetUsers_ApplicationUser_Id");
        }
        
        public override void Down()
        {
        }
    }
}
