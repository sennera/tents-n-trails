namespace TentsNTrails.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateEventComment4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.EventComments", new[] { "Author_Id" });
            AlterColumn("dbo.EventComments", "Author_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.EventComments", "Author_Id");
            AddForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.EventComments", new[] { "Author_Id" });
            AlterColumn("dbo.EventComments", "Author_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.EventComments", "Author_Id");
            AddForeignKey("dbo.EventComments", "Author_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
