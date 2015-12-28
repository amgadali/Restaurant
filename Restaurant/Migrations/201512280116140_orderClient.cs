namespace Restaurant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Client_Id", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Client_Id");
        }
    }
}
