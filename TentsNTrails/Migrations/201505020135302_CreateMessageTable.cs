namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMessageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        MessageText = c.String(nullable: false, maxLength: 1000),
                        TimeSent = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        FromUser_Id = c.String(maxLength: 128),
                        ToUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUser_Id)
                .Index(t => t.FromUser_Id)
                .Index(t => t.ToUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ToUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "FromUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "ToUser_Id" });
            DropIndex("dbo.Messages", new[] { "FromUser_Id" });
            DropTable("dbo.Messages");
        }
    }
}
