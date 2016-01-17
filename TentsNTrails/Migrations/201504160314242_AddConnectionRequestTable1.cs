namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConnectionRequestTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConnectionRequests", "Sender_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ConnectionRequests", "Sender_Id");
            AddForeignKey("dbo.ConnectionRequests", "Sender_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConnectionRequests", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ConnectionRequests", new[] { "Sender_Id" });
            DropColumn("dbo.ConnectionRequests", "Sender_Id");
        }
    }
}
