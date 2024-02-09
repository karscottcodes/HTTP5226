namespace PassionProject_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeam : DbMigration
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
            
            AddColumn("dbo.Players", "Team_TeamId", c => c.Int());
            CreateIndex("dbo.Players", "Team_TeamId");
            AddForeignKey("dbo.Players", "Team_TeamId", "dbo.Teams", "TeamId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "Team_TeamId" });
            DropColumn("dbo.Players", "Team_TeamId");
            DropTable("dbo.Teams");
        }
    }
}
