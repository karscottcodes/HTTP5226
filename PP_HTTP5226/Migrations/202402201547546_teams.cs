namespace PP_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teams : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                    })
                .PrimaryKey(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Teams");
        }
    }
}
