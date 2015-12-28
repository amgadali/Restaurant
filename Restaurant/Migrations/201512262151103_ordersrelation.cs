namespace Restaurant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ordersrelation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MenueItems", newName: "MenuItems");
            DropForeignKey("dbo.MenueItems", "Order_ID", "dbo.Orders");
            DropIndex("dbo.MenuItems", new[] { "Order_ID" });
            DropIndex("dbo.UserAddresses", new[] { "ApplicationUser_Id" });
            DropPrimaryKey("dbo.UserAddresses");
            CreateTable(
                "dbo.OrderMenuItems",
                c => new
                    {
                        Order_Id = c.Int(nullable: false),
                        MenuItem_Id = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_Id, t.MenuItem_Id })
                .ForeignKey("dbo.MenuItems", t => t.MenuItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .Index(t => t.Order_Id)
                .Index(t => t.MenuItem_Id);
            
            AlterColumn("dbo.UserAddresses", "ID", c => c.Int(nullable: false));
            AlterColumn("dbo.UserAddresses", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.UserAddresses", "ApplicationUser_Id");
            CreateIndex("dbo.UserAddresses", "ApplicationUser_Id");
            DropColumn("dbo.MenuItems", "Order_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MenuItems", "Order_ID", c => c.Int());
            DropForeignKey("dbo.OrderMenuItems", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderMenuItems", "MenuItem_Id", "dbo.MenuItems");
            DropIndex("dbo.UserAddresses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OrderMenuItems", new[] { "MenuItem_Id" });
            DropIndex("dbo.OrderMenuItems", new[] { "Order_Id" });
            DropPrimaryKey("dbo.UserAddresses");
            AlterColumn("dbo.UserAddresses", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserAddresses", "ID", c => c.Int(nullable: false, identity: true));
            DropTable("dbo.OrderMenuItems");
            AddPrimaryKey("dbo.UserAddresses", "ID");
            CreateIndex("dbo.UserAddresses", "ApplicationUser_Id");
            CreateIndex("dbo.MenuItems", "Order_ID");
            AddForeignKey("dbo.MenueItems", "Order_ID", "dbo.Orders", "ID");
            RenameTable(name: "dbo.MenuItems", newName: "MenueItems");
        }
    }
}
