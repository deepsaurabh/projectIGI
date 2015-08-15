namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_Existing_Table : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Enterprise.tblCourseDocument", newName: "tblAttachedDocument");
            DropForeignKey("Enterprise.tblToolkitDocument", "ToolkitId", "Enterprise.tblToolkit");
            RenameColumn(table: "Enterprise.tblAttachedDocument", name: "CourseDocumentId", newName: "AttachedDocumentId");
            CreateTable(
                "Enterprise.tblToolkitAttachmentMapping",
                c => new
                    {
                        ToolkitAttachmentMappingId = c.Long(nullable: false, identity: true),
                        toolkitId = c.Long(nullable: false),
                        toolkitDocumentID = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ToolkitAttachmentMappingId)
                .ForeignKey("Enterprise.tblToolkit", t => t.toolkitId, cascadeDelete: true)
                .ForeignKey("Enterprise.tblAttachedDocument", t => t.toolkitDocumentID, cascadeDelete: true)
                .Index(t => t.toolkitId)
                .Index(t => t.toolkitDocumentID);
            
            AddColumn("Enterprise.tblToolkit", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("Enterprise.tblToolkit", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("Enterprise.tblToolkitAttachmentMapping", "toolkitDocumentID", "Enterprise.tblAttachedDocument");
            DropForeignKey("Enterprise.tblToolkitAttachmentMapping", "toolkitId", "Enterprise.tblToolkit");
            DropIndex("Enterprise.tblToolkitAttachmentMapping", new[] { "toolkitDocumentID" });
            DropIndex("Enterprise.tblToolkitAttachmentMapping", new[] { "toolkitId" });
            DropColumn("Enterprise.tblToolkit", "EndDate");
            DropColumn("Enterprise.tblToolkit", "StartDate");
            DropTable("Enterprise.tblToolkitAttachmentMapping");
            RenameColumn(table: "Enterprise.tblAttachedDocument", name: "AttachedDocumentId", newName: "CourseDocumentId");
            AddForeignKey("Enterprise.tblToolkitDocument", "ToolkitId", "Enterprise.tblToolkit", "ToolkitId", cascadeDelete: true);
            RenameTable(name: "Enterprise.tblAttachedDocument", newName: "tblCourseDocument");
        }
    }
}
