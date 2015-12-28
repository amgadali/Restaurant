namespace Restaurant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderUser : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Orders", "Client_Id");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUser_Id", newName: "Client_Id");
            AlterColumn("dbo.Orders", "Client_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "Client_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", new[] { "Client_Id" });
            AlterColumn("dbo.Orders", "Client_Id", c => c.String());
            RenameColumn(table: "dbo.Orders", name: "Client_Id", newName: "ApplicationUser_Id");
            AddColumn("dbo.Orders", "Client_Id", c => c.String());
            CreateIndex("dbo.Orders", "ApplicationUser_Id");
        }
    }
}
