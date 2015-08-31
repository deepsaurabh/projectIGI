namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "tblUser",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 255, unicode: false),
                        LastName = c.String(maxLength: 255, unicode: false),
                        Gender = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false, precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "tblCourse",
                c => new
                    {
                        CourseId = c.Long(nullable: false, identity: true),
                        CourseName = c.String(maxLength: 255, unicode: false),
                        CourseFreeContent = c.String(maxLength: 1000, unicode: false),
                        CoursePublicContent = c.String(maxLength: 1000, unicode: false),
                        CoursePaidContent = c.String(maxLength: 1000, unicode: false),
                        Price = c.String(maxLength: 50, unicode: false),
                        CurrencyType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "tblCourseAttachmentMapping",
                c => new
                    {
                        CourseAttachmentMappingId = c.Long(nullable: false, identity: true),
                        courseId = c.Long(nullable: false),
                        courseDocumentID = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.CourseAttachmentMappingId)
                .ForeignKey("tblCourse", t => t.courseId, cascadeDelete: true)
                .ForeignKey("tblAttachedDocument", t => t.courseDocumentID, cascadeDelete: true)
                .Index(t => t.courseId)
                .Index(t => t.courseDocumentID);
            
            CreateTable(
                "tblAttachedDocument",
                c => new
                    {
                        AttachedDocumentId = c.Long(nullable: false, identity: true),
                        FileType = c.String(unicode: false),
                        FileSize = c.Long(),
                        FileData = c.Binary(),
                        DocumentType = c.Int(nullable: false),
                        DocumentName = c.String(maxLength: 255, unicode: false),
                        DocumentPseudoName = c.String(maxLength: 255, unicode: false),
                        Scope = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.AttachedDocumentId);
            
            CreateTable(
                "tblToolkitAttachmentMapping",
                c => new
                    {
                        ToolkitAttachmentMappingId = c.Long(nullable: false, identity: true),
                        toolkitId = c.Long(nullable: false),
                        toolkitDocumentID = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.ToolkitAttachmentMappingId)
                .ForeignKey("tblToolkit", t => t.toolkitId, cascadeDelete: true)
                .ForeignKey("tblAttachedDocument", t => t.toolkitDocumentID, cascadeDelete: true)
                .Index(t => t.toolkitId)
                .Index(t => t.toolkitDocumentID);
            
            CreateTable(
                "tblToolkit",
                c => new
                    {
                        ToolkitId = c.Long(nullable: false, identity: true),
                        ToolkitName = c.String(maxLength: 255, unicode: false),
                        ToolkitFreeContent = c.String(maxLength: 1000, unicode: false),
                        ToolkitPublicContent = c.String(maxLength: 1000, unicode: false),
                        ToolkitPaidContent = c.String(maxLength: 1000, unicode: false),
                        Price = c.String(maxLength: 50, unicode: false),
                        CurrencyType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.ToolkitId);
            
            CreateTable(
                "tblToolkitDocument",
                c => new
                    {
                        ToolkitDocumentId = c.Long(nullable: false, identity: true),
                        ToolkitId = c.Long(nullable: false),
                        DocumentType = c.Int(nullable: false),
                        DocumentPath = c.String(unicode: false),
                        DocumentName = c.String(unicode: false),
                        DocumentPseudoName = c.String(unicode: false),
                        Scope = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.ToolkitDocumentId)
                .ForeignKey("tblToolkit", t => t.ToolkitId, cascadeDelete: true)
                .Index(t => t.ToolkitId);
            
            CreateTable(
                "tblUserPurchaseHistory",
                c => new
                    {
                        UserPurchaseHistoryId = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        ItemType = c.Int(nullable: false),
                        ItemCount = c.Int(nullable: false),
                        AmountPaid = c.String(maxLength: 255, unicode: false),
                        TransactionDate = c.DateTime(nullable: false, precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.UserPurchaseHistoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("tblToolkitDocument", "ToolkitId", "tblToolkit");
            DropForeignKey("tblToolkitAttachmentMapping", "toolkitDocumentID", "tblAttachedDocument");
            DropForeignKey("tblToolkitAttachmentMapping", "toolkitId", "tblToolkit");
            DropForeignKey("tblCourseAttachmentMapping", "courseDocumentID", "tblAttachedDocument");
            DropForeignKey("tblCourseAttachmentMapping", "courseId", "tblCourse");
            DropIndex("tblToolkitDocument", new[] { "ToolkitId" });
            DropIndex("tblToolkitAttachmentMapping", new[] { "toolkitDocumentID" });
            DropIndex("tblToolkitAttachmentMapping", new[] { "toolkitId" });
            DropIndex("tblCourseAttachmentMapping", new[] { "courseDocumentID" });
            DropIndex("tblCourseAttachmentMapping", new[] { "courseId" });
            DropTable("tblUserPurchaseHistory");
            DropTable("tblToolkitDocument");
            DropTable("tblToolkit");
            DropTable("tblToolkitAttachmentMapping");
            DropTable("tblAttachedDocument");
            DropTable("tblCourseAttachmentMapping");
            DropTable("tblCourse");
            DropTable("tblUser");
        }
    }
}
