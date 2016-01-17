namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAll : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.ConnectionRequests",
                c => new
                    {
                        ConnectionRequestID = c.Int(nullable: false, identity: true),
                        RequestedUser_Id = c.String(maxLength: 128),
                        Sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ConnectionRequestID)
                .ForeignKey("dbo.AspNetUsers", t => t.RequestedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .Index(t => t.RequestedUser_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EnrollmentDate = c.DateTime(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Private = c.Boolean(nullable: false),
                        About = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.LocationFlags",
                c => new
                    {
                        FlagID = c.Int(nullable: false, identity: true),
                        LocationID = c.Int(nullable: false),
                        Flag = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                        User_Id1 = c.String(maxLength: 128),
                        User_Id2 = c.String(maxLength: 128),
                        User_Id3 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FlagID)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id1)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id2)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id3)
                .Index(t => t.LocationID)
                .Index(t => t.User_Id)
                .Index(t => t.User_Id1)
                .Index(t => t.User_Id2)
                .Index(t => t.User_Id3);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        Label = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        Difficulty = c.Int(nullable: false),
                        Description = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        AltText = c.String(),
                        ImageUrl = c.String(nullable: false),
                        DateTaken = c.DateTime(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        LocationID = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ImageID)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.LocationID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.LocationRecreations",
                c => new
                    {
                        LocationID = c.Int(nullable: false),
                        RecreationID = c.Int(nullable: false),
                        RecreationLabel = c.String(),
                        IsChecked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.LocationID, t.RecreationID })
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.Recreations", t => t.RecreationID, cascadeDelete: true)
                .Index(t => t.LocationID)
                .Index(t => t.RecreationID);
            
            CreateTable(
                "dbo.Recreations",
                c => new
                    {
                        RecreationID = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RecreationID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        LocationID = c.Int(nullable: false),
                        ReviewDate = c.DateTime(nullable: false),
                        Rating = c.Boolean(nullable: false),
                        Comment = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.LocationID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        VideoID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 200),
                        EmbedCode = c.String(nullable: false, maxLength: 2000),
                        LocationID = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.VideoID)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .Index(t => t.LocationID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRecreations",
                c => new
                    {
                        User = c.String(nullable: false, maxLength: 128),
                        RecreationID = c.Int(nullable: false),
                        RecreationLabel = c.String(),
                        IsChecked = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.User, t.RecreationID })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ConnectionID = c.Int(nullable: false, identity: true),
                        User1_Id = c.String(maxLength: 128),
                        User2_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ConnectionID)
                .ForeignKey("dbo.AspNetUsers", t => t.User1_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User2_Id)
                .Index(t => t.User1_Id)
                .Index(t => t.User2_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.RecreationLocations",
                c => new
                    {
                        Recreation_RecreationID = c.Int(nullable: false),
                        Location_LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recreation_RecreationID, t.Location_LocationID })
                .ForeignKey("dbo.Recreations", t => t.Recreation_RecreationID, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_LocationID, cascadeDelete: true)
                .Index(t => t.Recreation_RecreationID)
                .Index(t => t.Location_LocationID);
            
        }
        
        public override void Down()
        {
            
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Connections", "User2_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Connections", "User1_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConnectionRequests", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConnectionRequests", "RequestedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id3", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRecreations", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id2", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Videos", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.Reviews", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.LocationRecreations", "RecreationID", "dbo.Recreations");
            DropForeignKey("dbo.RecreationLocations", "Location_LocationID", "dbo.Locations");
            DropForeignKey("dbo.RecreationLocations", "Recreation_RecreationID", "dbo.Recreations");
            DropForeignKey("dbo.LocationRecreations", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.LocationFlags", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.Images", "LocationID", "dbo.Locations");
            DropIndex("dbo.RecreationLocations", new[] { "Location_LocationID" });
            DropIndex("dbo.RecreationLocations", new[] { "Recreation_RecreationID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Connections", new[] { "User2_Id" });
            DropIndex("dbo.Connections", new[] { "User1_Id" });
            DropIndex("dbo.UserRecreations", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Videos", new[] { "LocationID" });
            DropIndex("dbo.Reviews", new[] { "User_Id" });
            DropIndex("dbo.Reviews", new[] { "LocationID" });
            DropIndex("dbo.LocationRecreations", new[] { "RecreationID" });
            DropIndex("dbo.LocationRecreations", new[] { "LocationID" });
            DropIndex("dbo.Images", new[] { "User_Id" });
            DropIndex("dbo.Images", new[] { "LocationID" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id3" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id2" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id1" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id" });
            DropIndex("dbo.LocationFlags", new[] { "LocationID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ConnectionRequests", new[] { "Sender_Id" });
            DropIndex("dbo.ConnectionRequests", new[] { "RequestedUser_Id" });
            DropTable("dbo.RecreationLocations");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Connections");
            DropTable("dbo.UserRecreations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Videos");
            DropTable("dbo.Reviews");
            DropTable("dbo.Recreations");
            DropTable("dbo.LocationRecreations");
            DropTable("dbo.Images");
            DropTable("dbo.Locations");
            DropTable("dbo.LocationFlags");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ConnectionRequests");
            
        }
    }
}
