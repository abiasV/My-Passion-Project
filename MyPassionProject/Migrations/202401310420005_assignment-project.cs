namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assignmentproject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.Assignments", "ProjectId");
            AddForeignKey("dbo.Assignments", "ProjectId", "dbo.Projects", "ProjectId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Assignments", new[] { "ProjectId" });
            DropColumn("dbo.Assignments", "ProjectId");
        }
    }
}
