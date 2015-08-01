namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConstraint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Enterprise.tblUser", "FirstName", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("Enterprise.tblUser", "LastName", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("Enterprise.tblUser", "Gender", c => c.Int(nullable: false));
            AlterColumn("Enterprise.tblCourse", "CourseName", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("Enterprise.tblCourse", "CourseFreeContent", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("Enterprise.tblCourse", "CoursePublicContent", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("Enterprise.tblCourse", "CoursePaidContent", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("Enterprise.tblCourse", "Price", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("Enterprise.tblCourseDocument", "DocumentPath", c => c.String(maxLength: 1023, unicode: false));
            AlterColumn("Enterprise.tblCourseDocument", "DocumentName", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("Enterprise.tblCourseDocument", "DocumentPseudoName", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("Enterprise.tblToolkit", "ToolkitName", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("Enterprise.tblToolkit", "ToolkitFreeContent", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("Enterprise.tblToolkit", "ToolkitPublicContent", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("Enterprise.tblToolkit", "ToolkitPaidContent", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("Enterprise.tblToolkit", "Price", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("Enterprise.tblToolkit", "Price", c => c.String());
            AlterColumn("Enterprise.tblToolkit", "ToolkitPaidContent", c => c.String());
            AlterColumn("Enterprise.tblToolkit", "ToolkitPublicContent", c => c.String());
            AlterColumn("Enterprise.tblToolkit", "ToolkitFreeContent", c => c.String());
            AlterColumn("Enterprise.tblToolkit", "ToolkitName", c => c.String());
            AlterColumn("Enterprise.tblCourseDocument", "DocumentPseudoName", c => c.String());
            AlterColumn("Enterprise.tblCourseDocument", "DocumentName", c => c.String());
            AlterColumn("Enterprise.tblCourseDocument", "DocumentPath", c => c.String());
            AlterColumn("Enterprise.tblCourse", "Price", c => c.String());
            AlterColumn("Enterprise.tblCourse", "CoursePaidContent", c => c.String());
            AlterColumn("Enterprise.tblCourse", "CoursePublicContent", c => c.String());
            AlterColumn("Enterprise.tblCourse", "CourseFreeContent", c => c.String());
            AlterColumn("Enterprise.tblCourse", "CourseName", c => c.String());
            AlterColumn("Enterprise.tblUser", "Gender", c => c.String());
            AlterColumn("Enterprise.tblUser", "LastName", c => c.String());
            AlterColumn("Enterprise.tblUser", "FirstName", c => c.String());
        }
    }
}
