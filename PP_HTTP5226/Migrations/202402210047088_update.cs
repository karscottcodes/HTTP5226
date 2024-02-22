namespace PP_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Players", "PlayerDob", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Players", "PlayerJoin", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Players", "PlayerJoin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Players", "PlayerDob", c => c.DateTime(nullable: false));
        }
    }
}
