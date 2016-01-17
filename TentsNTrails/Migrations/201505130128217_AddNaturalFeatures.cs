namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNaturalFeatures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationFeatures",
                c => new
                    {
                        LocationID = c.Int(nullable: false),
                        NaturalFeatureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LocationID, t.NaturalFeatureID })
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.NaturalFeatures", t => t.NaturalFeatureID, cascadeDelete: true)
                .Index(t => t.LocationID)
                .Index(t => t.NaturalFeatureID);
            
            CreateTable(
                "dbo.NaturalFeatures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocationFeatures", "NaturalFeatureID", "dbo.NaturalFeatures");
            DropForeignKey("dbo.LocationFeatures", "LocationID", "dbo.Locations");
            DropIndex("dbo.LocationFeatures", new[] { "NaturalFeatureID" });
            DropIndex("dbo.LocationFeatures", new[] { "LocationID" });
            DropTable("dbo.NaturalFeatures");
            DropTable("dbo.LocationFeatures");
        }
    }
}
