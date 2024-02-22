namespace PP_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "TeamName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "TeamName");
        }
    }
}
