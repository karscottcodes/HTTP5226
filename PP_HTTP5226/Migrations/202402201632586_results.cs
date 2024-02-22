namespace PP_HTTP5226.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class results : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        ResultDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ResultNote = c.String(),
                        Team1Id = c.Int(nullable: false),
                        Team1Score = c.Int(nullable: false),
                        Team2Id = c.Int(nullable: false),
                        Team2Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResultId)
                .ForeignKey("dbo.Teams", t => t.Team1Id)
                .ForeignKey("dbo.Teams", t => t.Team2Id)
                .Index(t => t.Team1Id)
                .Index(t => t.Team2Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Results", "Team2Id", "dbo.Teams");
            DropForeignKey("dbo.Results", "Team1Id", "dbo.Teams");
            DropIndex("dbo.Results", new[] { "Team2Id" });
            DropIndex("dbo.Results", new[] { "Team1Id" });
            DropTable("dbo.Results");
        }
    }
}
