namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseAndToolkit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Enterprise.tblCourse",
                c => new
                    {
                        CourseId = c.Long(nullable: false, identity: true),
                        CourseName = c.String(),
                        CourseFreeContent = c.String(),
                        CoursePublicContent = c.String(),
                        CoursePaidContent = c.String(),
                        Price = c.String(),
                        CurrencyType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "Enterprise.tblCourseDocument",
                c => new
                    {
                        CourseDocumentId = c.Long(nullable: false, identity: true),
                        CourseId = c.Long(nullable: false),
                        DocumentType = c.Int(nullable: false),
                        DocumentPath = c.String(),
                        DocumentName = c.String(),
                        DocumentPseudoName = c.String(),
                        Scope = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CourseDocumentId)
                .ForeignKey("Enterprise.tblCourse", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "Enterprise.tblToolkit",
                c => new
                    {
                        ToolkitId = c.Long(nullable: false, identity: true),
                        ToolkitName = c.String(),
                        ToolkitFreeContent = c.String(),
                        ToolkitPublicContent = c.String(),
                        ToolkitPaidContent = c.String(),
                        Price = c.String(),
                        CurrencyType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ToolkitId);
            
            CreateTable(
                "Enterprise.tblToolkitDocument",
                c => new
                    {
                        ToolkitDocumentId = c.Long(nullable: false, identity: true),
                        ToolkitId = c.Long(nullable: false),
                        DocumentType = c.Int(nullable: false),
                        DocumentPath = c.String(),
                        DocumentName = c.String(),
                        DocumentPseudoName = c.String(),
                        Scope = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ToolkitDocumentId)
                .ForeignKey("Enterprise.tblToolkit", t => t.ToolkitId, cascadeDelete: true)
                .Index(t => t.ToolkitId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Enterprise.tblToolkitDocument", "ToolkitId", "Enterprise.tblToolkit");
            DropForeignKey("Enterprise.tblCourseDocument", "CourseId", "Enterprise.tblCourse");
            DropIndex("Enterprise.tblToolkitDocument", new[] { "ToolkitId" });
            DropIndex("Enterprise.tblCourseDocument", new[] { "CourseId" });
            DropTable("Enterprise.tblToolkitDocument");
            DropTable("Enterprise.tblToolkit");
            DropTable("Enterprise.tblCourseDocument");
            DropTable("Enterprise.tblCourse");
        }
    }
}
