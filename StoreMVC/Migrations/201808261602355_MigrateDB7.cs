namespace StoreMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB7 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Customers", new[] { "Name" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Customers", "Name", unique: true);
        }
    }
}
