namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateEventComment3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.EventComments", name: "Event_EventID", newName: "EventID");
            RenameIndex(table: "dbo.EventComments", name: "IX_Event_EventID", newName: "IX_EventID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EventComments", name: "IX_EventID", newName: "IX_Event_EventID");
            RenameColumn(table: "dbo.EventComments", name: "EventID", newName: "Event_EventID");
        }
    }
}
