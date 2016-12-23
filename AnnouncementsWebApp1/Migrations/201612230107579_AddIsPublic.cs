namespace AnnouncementsWebApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPublic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "IsPublic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Announcements", "IsDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Announcements", "IsDeleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Announcements", "IsPublic");
        }
    }
}
