namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Enterprise.tblUser",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("Enterprise.tblUser");
        }
    }
}
