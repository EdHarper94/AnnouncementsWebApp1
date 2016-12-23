namespace AnnouncementsWebApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Announcements", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Announcements", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Announcements", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.Comments", "Text", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Text", c => c.String());
            AlterColumn("dbo.Announcements", "Text", c => c.String());
            AlterColumn("dbo.Announcements", "Description", c => c.String());
            AlterColumn("dbo.Announcements", "Title", c => c.String());
        }
    }
}
