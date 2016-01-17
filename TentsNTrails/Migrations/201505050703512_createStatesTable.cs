namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createStatesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateID = c.String(nullable: false, maxLength: 2),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.StateID);
            
            AddColumn("dbo.Locations", "StateID", c => c.String(maxLength: 2));
            CreateIndex("dbo.Locations", "StateID");
            AddForeignKey("dbo.Locations", "StateID", "dbo.States", "StateID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "StateID", "dbo.States");
            DropIndex("dbo.Locations", new[] { "StateID" });
            DropColumn("dbo.Locations", "StateID");
            DropTable("dbo.States");
        }
    }
}
