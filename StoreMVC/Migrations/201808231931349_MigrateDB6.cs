namespace StoreMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB6 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Customers", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "Name" });
        }
    }
}
