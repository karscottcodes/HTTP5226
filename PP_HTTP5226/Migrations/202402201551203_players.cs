namespace PP_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class players : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(),
                        PlayerDob = c.DateTime(nullable: false),
                        PlayerJoin = c.DateTime(nullable: false),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerId)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "TeamId", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "TeamId" });
            DropTable("dbo.Players");
        }
    }
}
