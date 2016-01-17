namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateEventComment : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.EventComments");
            DropColumn("dbo.EventComments", "EventID");
            AddColumn("dbo.EventComments", "EventCommentID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.EventComments", "EventCommentID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventComments", "EventID", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.EventComments");
            DropColumn("dbo.EventComments", "EventCommentID");
            AddPrimaryKey("dbo.EventComments", "EventID");
        }
    }
}
