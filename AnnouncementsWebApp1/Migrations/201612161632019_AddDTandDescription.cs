namespace AnnouncementsWebApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDTandDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Announcements", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "Description");
            DropColumn("dbo.Announcements", "CreatedDate");
        }
    }
}
