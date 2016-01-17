namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConnectionRequestTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "User_Id", "dbo.AspNetUsers");
            CreateTable(
                "dbo.ConnectionRequests",
                c => new
                    {
                        ConnectionRequestID = c.Int(nullable: false, identity: true),
                        Sender_Id = c.String(maxLength: 128),
                        User1_Id = c.String(maxLength: 128),
                        User2_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ConnectionRequestID)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User1_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User2_Id)
                .Index(t => t.Sender_Id)
                .Index(t => t.User1_Id)
                .Index(t => t.User2_Id);
            
            AddColumn("dbo.Images", "User_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.LocationFlags", "User_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.LocationFlags", "User_Id2", c => c.String(maxLength: 128));
            AddColumn("dbo.LocationFlags", "User_Id3", c => c.String(maxLength: 128));
            CreateIndex("dbo.LocationFlags", "User_Id1");
            CreateIndex("dbo.LocationFlags", "User_Id2");
            CreateIndex("dbo.LocationFlags", "User_Id3");
            CreateIndex("dbo.Images", "User_Id1");
            AddForeignKey("dbo.LocationFlags", "User_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.LocationFlags", "User_Id2", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Images", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.LocationFlags", "User_Id3", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Images", "User_Id1", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "User_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConnectionRequests", "User2_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConnectionRequests", "User1_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConnectionRequests", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id3", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id2", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Images", new[] { "User_Id1" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id3" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id2" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id1" });
            DropIndex("dbo.ConnectionRequests", new[] { "User2_Id" });
            DropIndex("dbo.ConnectionRequests", new[] { "User1_Id" });
            DropIndex("dbo.ConnectionRequests", new[] { "Sender_Id" });
            DropColumn("dbo.LocationFlags", "User_Id3");
            DropColumn("dbo.LocationFlags", "User_Id2");
            DropColumn("dbo.LocationFlags", "User_Id1");
            DropColumn("dbo.Images", "User_Id1");
            DropTable("dbo.ConnectionRequests");
            AddForeignKey("dbo.Images", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
