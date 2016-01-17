namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sprint9start : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateRead = c.DateTime(),
                        IsRead = c.Boolean(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NotificationID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
        }
       
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "UserID" });
            DropTable("dbo.Notifications");
        }
    }
}
