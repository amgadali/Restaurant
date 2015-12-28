namespace Restaurant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderstatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Orders", "Status_ID", c => c.Int());
            CreateIndex("dbo.Orders", "Status_ID");
            AddForeignKey("dbo.Orders", "Status_ID", "dbo.OrderStatus", "ID");
            DropColumn("dbo.Orders", "OrderStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            DropForeignKey("dbo.Orders", "Status_ID", "dbo.OrderStatus");
            DropIndex("dbo.Orders", new[] { "Status_ID" });
            DropColumn("dbo.Orders", "Status_ID");
            DropTable("dbo.OrderStatus");
        }
    }
}
