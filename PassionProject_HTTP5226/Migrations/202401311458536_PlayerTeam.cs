namespace PassionProject_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerTeam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Players", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "Team_TeamId" });
            RenameColumn(table: "dbo.Players", name: "Team_TeamId", newName: "TeamId");
            AlterColumn("dbo.Players", "TeamId", c => c.Int(nullable: true));
            CreateIndex("dbo.Players", "TeamId");
            AddForeignKey("dbo.Players", "TeamId", "dbo.Teams", "TeamId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "TeamId", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "TeamId" });
            AlterColumn("dbo.Players", "TeamId", c => c.Int());
            RenameColumn(table: "dbo.Players", name: "TeamId", newName: "Team_TeamId");
            CreateIndex("dbo.Players", "Team_TeamId");
            AddForeignKey("dbo.Players", "Team_TeamId", "dbo.Teams", "TeamId");
        }
    }
}
