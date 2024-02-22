namespace PP_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Teams", "TeamName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teams", "TeamName", c => c.String());
        }
    }
}
