namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_CourseAttachmentMapping : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Enterprise.tblCourseDocument", "Course_Id", "Enterprise.tblCourse");
            DropIndex("Enterprise.tblCourseDocument", new[] { "Course_Id" });
            CreateTable(
                "Enterprise.tblCourseAttachmentMapping",
                c => new
                    {
                        CourseAttachmentMappingId = c.Long(nullable: false, identity: true),
                        courseId = c.Long(nullable: false),
                        courseDocumentID = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CourseAttachmentMappingId)
                .ForeignKey("Enterprise.tblCourse", t => t.courseId, cascadeDelete: true)
                .ForeignKey("Enterprise.tblCourseDocument", t => t.courseDocumentID, cascadeDelete: true)
                .Index(t => t.courseId)
                .Index(t => t.courseDocumentID);
            
            DropColumn("Enterprise.tblCourseDocument", "Course_Id");
        }
        
        public override void Down()
        {
            AddColumn("Enterprise.tblCourseDocument", "Course_Id", c => c.Long());
            DropForeignKey("Enterprise.tblCourseAttachmentMapping", "courseDocumentID", "Enterprise.tblCourseDocument");
            DropForeignKey("Enterprise.tblCourseAttachmentMapping", "courseId", "Enterprise.tblCourse");
            DropIndex("Enterprise.tblCourseAttachmentMapping", new[] { "courseDocumentID" });
            DropIndex("Enterprise.tblCourseAttachmentMapping", new[] { "courseId" });
            DropTable("Enterprise.tblCourseAttachmentMapping");
            CreateIndex("Enterprise.tblCourseDocument", "Course_Id");
            AddForeignKey("Enterprise.tblCourseDocument", "Course_Id", "Enterprise.tblCourse", "CourseId");
        }
    }
}
