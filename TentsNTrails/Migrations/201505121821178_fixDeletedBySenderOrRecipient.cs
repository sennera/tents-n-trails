namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixDeletedBySenderOrRecipient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "DeletedBySender", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "DeletedByRecipient", c => c.Boolean(nullable: false));
            DropColumn("dbo.Messages", "DeletedSender");
            DropColumn("dbo.Messages", "DeletedRecipient");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "DeletedRecipient", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "DeletedSender", c => c.Boolean(nullable: false));
            DropColumn("dbo.Messages", "DeletedByRecipient");
            DropColumn("dbo.Messages", "DeletedBySender");
        }
    }
}
