namespace Restaurant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagefield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MenuItems", "Image", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MenuItems", "Image", c => c.Binary());
        }
    }
}
