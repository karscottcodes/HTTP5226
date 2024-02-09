namespace PassionProject_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Result : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        ResultDate = c.DateTime(nullable: false),
                        ResultNote = c.String(),
                    })
                .PrimaryKey(t => t.ResultId);
            
            CreateTable(
                "dbo.ResultTeams",
                c => new
                    {
                        ResultTeamId = c.Int(nullable: false, identity: true),
                        ResultId = c.Int(nullable: false),
                        Team1Id = c.Int(nullable: false),
                        Team1Score = c.Int(nullable: false),
                        Team2Id = c.Int(nullable: false),
                        Team2Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResultTeamId)
                .ForeignKey("dbo.Results", t => t.ResultId)
                .ForeignKey("dbo.Teams", t => t.Team1Id)
                .ForeignKey("dbo.Teams", t => t.Team2Id)
                .Index(t => t.ResultId)
                .Index(t => t.Team1Id)
                .Index(t => t.Team2Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResultTeams", "Team2Id", "dbo.Teams");
            DropForeignKey("dbo.ResultTeams", "Team1Id", "dbo.Teams");
            DropForeignKey("dbo.ResultTeams", "ResultId", "dbo.Results");
            DropIndex("dbo.ResultTeams", new[] { "Team2Id" });
            DropIndex("dbo.ResultTeams", new[] { "Team1Id" });
            DropIndex("dbo.ResultTeams", new[] { "ResultId" });
            DropTable("dbo.ResultTeams");
            DropTable("dbo.Results");
        }
    }
}
