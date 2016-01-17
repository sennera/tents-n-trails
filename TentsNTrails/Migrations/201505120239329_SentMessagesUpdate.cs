namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SentMessagesUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "DeletedSender", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "DeletedRecipient", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "DeletedRecipient");
            DropColumn("dbo.Messages", "DeletedSender");
        }
    }
}
