namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Enterprise.tblCart",
                c => new
                    {
                        CartId = c.Long(nullable: false, identity: true),
                        UserName = c.String(maxLength: 255, unicode: false),
                        ItemId = c.Long(nullable: false),
                        type = c.String(maxLength: 255, unicode: false),
                        quantity = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CartId);
            
        }
        
        public override void Down()
        {
            DropTable("Enterprise.tblCart");
        }
    }
}
