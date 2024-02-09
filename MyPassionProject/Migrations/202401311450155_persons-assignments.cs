namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class personsassignments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        PersonFirstName = c.String(),
                        PersonLastName = c.String(),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.PersonAssignments",
                c => new
                    {
                        Person_PersonId = c.Int(nullable: false),
                        Assignment_AssignmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_PersonId, t.Assignment_AssignmentId })
                .ForeignKey("dbo.People", t => t.Person_PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Assignments", t => t.Assignment_AssignmentId, cascadeDelete: true)
                .Index(t => t.Person_PersonId)
                .Index(t => t.Assignment_AssignmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonAssignments", "Assignment_AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.PersonAssignments", "Person_PersonId", "dbo.People");
            DropIndex("dbo.PersonAssignments", new[] { "Assignment_AssignmentId" });
            DropIndex("dbo.PersonAssignments", new[] { "Person_PersonId" });
            DropTable("dbo.PersonAssignments");
            DropTable("dbo.People");
        }
    }
}
