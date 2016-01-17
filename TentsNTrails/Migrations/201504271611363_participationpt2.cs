namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class participationpt2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventParticipants",
                c => new
                    {
                        EventParticipationID = c.Int(nullable: false, identity: true),
                        Event_EventID = c.Int(),
                        Participant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EventParticipationID)
                .ForeignKey("dbo.Events", t => t.Event_EventID)
                .ForeignKey("dbo.AspNetUsers", t => t.Participant_Id)
                .Index(t => t.Event_EventID)
                .Index(t => t.Participant_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        LocationID = c.Int(nullable: false),
                        Organizer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Organizer_Id)
                .Index(t => t.LocationID)
                .Index(t => t.Organizer_Id);
            
            CreateTable(
                "dbo.EventComments",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        Comment = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Events_EventID = c.Int(),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Events", t => t.Events_EventID)
                .Index(t => t.Author_Id)
                .Index(t => t.Events_EventID);
            
            AddColumn("dbo.Videos", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Videos", "User_Id");
            AddForeignKey("dbo.Videos", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventParticipants", "Participant_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventParticipants", "Event_EventID", "dbo.Events");
            DropForeignKey("dbo.Events", "Organizer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.EventComments", "Events_EventID", "dbo.Events");
            DropForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Videos", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.EventComments", new[] { "Events_EventID" });
            DropIndex("dbo.EventComments", new[] { "Author_Id" });
            DropIndex("dbo.Events", new[] { "Organizer_Id" });
            DropIndex("dbo.Events", new[] { "LocationID" });
            DropIndex("dbo.EventParticipants", new[] { "Participant_Id" });
            DropIndex("dbo.EventParticipants", new[] { "Event_EventID" });
            DropIndex("dbo.Videos", new[] { "User_Id" });
            DropColumn("dbo.Videos", "User_Id");
            DropTable("dbo.EventComments");
            DropTable("dbo.Events");
            DropTable("dbo.EventParticipants");
        }
    }
}
