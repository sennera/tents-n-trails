namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateEventComment2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventComments", "Events_EventID", "dbo.Events");
            DropIndex("dbo.EventComments", new[] { "Author_Id" });
            DropIndex("dbo.EventComments", new[] { "Events_EventID" });
            RenameColumn(table: "dbo.EventComments", name: "Events_EventID", newName: "Event_EventID");
            AlterColumn("dbo.EventComments", "Author_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.EventComments", "Event_EventID", c => c.Int(nullable: false));
            CreateIndex("dbo.EventComments", "Author_Id");
            CreateIndex("dbo.EventComments", "Event_EventID");
            AddForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EventComments", "Event_EventID", "dbo.Events", "EventID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventComments", "Event_EventID", "dbo.Events");
            DropForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.EventComments", new[] { "Event_EventID" });
            DropIndex("dbo.EventComments", new[] { "Author_Id" });
            AlterColumn("dbo.EventComments", "Event_EventID", c => c.Int());
            AlterColumn("dbo.EventComments", "Author_Id", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.EventComments", name: "Event_EventID", newName: "Events_EventID");
            CreateIndex("dbo.EventComments", "Events_EventID");
            CreateIndex("dbo.EventComments", "Author_Id");
            AddForeignKey("dbo.EventComments", "Events_EventID", "dbo.Events", "EventID");
            AddForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
