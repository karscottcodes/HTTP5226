namespace PassionProject_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Results", "ResultDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Results", "ResultDate", c => c.DateTime(nullable: false));
        }
    }
}
