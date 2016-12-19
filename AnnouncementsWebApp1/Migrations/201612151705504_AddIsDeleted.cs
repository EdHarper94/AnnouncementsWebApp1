namespace AnnouncementsWebApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcements", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "IsDeleted");
        }
    }
}
