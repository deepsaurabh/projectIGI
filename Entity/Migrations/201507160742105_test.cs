namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Test.tblTestClass",
                c => new
                    {
                        TestClassId = c.Long(nullable: false, identity: true),
                        Wow = c.String(),
                        Hey = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TestClassId);
            
        }
        
        public override void Down()
        {
            DropTable("Test.tblTestClass");
        }
    }
}
