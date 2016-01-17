namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConnectionRequestTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "User_Id", "dbo.AspNetUsers");
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
            DropForeignKey("dbo.LocationFlags", "User_Id3", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id2", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationFlags", "User_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Images", new[] { "User_Id1" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id3" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id2" });
            DropIndex("dbo.LocationFlags", new[] { "User_Id1" });
            DropColumn("dbo.LocationFlags", "User_Id3");
            DropColumn("dbo.LocationFlags", "User_Id2");
            DropColumn("dbo.LocationFlags", "User_Id1");
            DropColumn("dbo.Images", "User_Id1");
            AddForeignKey("dbo.Images", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
