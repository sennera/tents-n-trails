namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeStateFromLocation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Locations", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "State", c => c.String());
        }
    }
}
