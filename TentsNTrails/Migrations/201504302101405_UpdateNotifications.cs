namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Notifications", "PotentialFriend_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notifications", "PotentialFriend_Id");
            AddForeignKey("dbo.Notifications", "PotentialFriend_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "PotentialFriend_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "PotentialFriend_Id" });
            DropColumn("dbo.Notifications", "PotentialFriend_Id");
            DropColumn("dbo.Notifications", "Discriminator");
        }
    }
}
