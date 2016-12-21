namespace AnnouncementsWebApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeenAnnouncements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeenAnnouncements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnnouncementId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Announcements", t => t.AnnouncementId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.AnnouncementId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SeenAnnouncements", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SeenAnnouncements", "AnnouncementId", "dbo.Announcements");
            DropIndex("dbo.SeenAnnouncements", new[] { "User_Id" });
            DropIndex("dbo.SeenAnnouncements", new[] { "AnnouncementId" });
            DropTable("dbo.SeenAnnouncements");
        }
    }
}
