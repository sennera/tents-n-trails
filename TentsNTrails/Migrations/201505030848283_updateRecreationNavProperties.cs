namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateRecreationNavProperties : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UserRecreations", "RecreationID");
            AddForeignKey("dbo.UserRecreations", "RecreationID", "dbo.Recreations", "RecreationID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRecreations", "RecreationID", "dbo.Recreations");
            DropIndex("dbo.UserRecreations", new[] { "RecreationID" });
        }
    }
}
