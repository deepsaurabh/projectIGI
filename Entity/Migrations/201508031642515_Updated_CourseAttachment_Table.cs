namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_CourseAttachment_Table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Enterprise.tblCourseDocument", "CourseId", "Enterprise.tblCourse");
            DropIndex("Enterprise.tblCourseDocument", new[] { "CourseId" });
            RenameColumn(table: "Enterprise.tblCourseDocument", name: "CourseId", newName: "Course_Id");
            AddColumn("Enterprise.tblCourseDocument", "FileType", c => c.String());
            AddColumn("Enterprise.tblCourseDocument", "FileSize", c => c.Long());
            AddColumn("Enterprise.tblCourseDocument", "FileData", c => c.Binary());
            AlterColumn("Enterprise.tblCourseDocument", "Course_Id", c => c.Long());
            CreateIndex("Enterprise.tblCourseDocument", "Course_Id");
            AddForeignKey("Enterprise.tblCourseDocument", "Course_Id", "Enterprise.tblCourse", "CourseId");
            DropColumn("Enterprise.tblCourseDocument", "DocumentPath");
        }
        
        public override void Down()
        {
            AddColumn("Enterprise.tblCourseDocument", "DocumentPath", c => c.String(maxLength: 1023, unicode: false));
            DropForeignKey("Enterprise.tblCourseDocument", "Course_Id", "Enterprise.tblCourse");
            DropIndex("Enterprise.tblCourseDocument", new[] { "Course_Id" });
            AlterColumn("Enterprise.tblCourseDocument", "Course_Id", c => c.Long(nullable: false));
            DropColumn("Enterprise.tblCourseDocument", "FileData");
            DropColumn("Enterprise.tblCourseDocument", "FileSize");
            DropColumn("Enterprise.tblCourseDocument", "FileType");
            RenameColumn(table: "Enterprise.tblCourseDocument", name: "Course_Id", newName: "CourseId");
            CreateIndex("Enterprise.tblCourseDocument", "CourseId");
            AddForeignKey("Enterprise.tblCourseDocument", "CourseId", "Enterprise.tblCourse", "CourseId", cascadeDelete: true);
        }
    }
}
