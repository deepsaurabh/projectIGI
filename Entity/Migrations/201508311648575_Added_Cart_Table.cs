namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Cart_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "tblCart",
                c => new
                    {
                        CartId = c.Long(nullable: false, identity: true),
                        UserName = c.String(maxLength: 255, unicode: false),
                        ItemId = c.Long(nullable: false),
                        type = c.String(maxLength: 255, unicode: false),
                        quantity = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.CartId);
            
        }
        
        public override void Down()
        {
            DropTable("tblCart");
        }
    }
}
