namespace AnnouncementsWebApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCommentsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Text");
        }
    }
}
