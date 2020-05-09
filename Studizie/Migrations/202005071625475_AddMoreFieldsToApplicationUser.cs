namespace Studizie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFieldsToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsScience", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsArts", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsTechnology", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsEducation", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsSports", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsReligious", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsSkillAcquisition", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsEntrepreneurship", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsGames", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsGames");
            DropColumn("dbo.AspNetUsers", "IsEntrepreneurship");
            DropColumn("dbo.AspNetUsers", "IsSkillAcquisition");
            DropColumn("dbo.AspNetUsers", "IsReligious");
            DropColumn("dbo.AspNetUsers", "IsSports");
            DropColumn("dbo.AspNetUsers", "IsEducation");
            DropColumn("dbo.AspNetUsers", "IsTechnology");
            DropColumn("dbo.AspNetUsers", "IsArts");
            DropColumn("dbo.AspNetUsers", "IsScience");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
