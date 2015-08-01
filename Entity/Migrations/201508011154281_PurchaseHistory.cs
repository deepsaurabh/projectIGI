namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Enterprise.tblUserPurchaseHistory",
                c => new
                    {
                        UserPurchaseHistoryId = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        ItemType = c.Int(nullable: false),
                        ItemCount = c.Int(nullable: false),
                        AmountPaid = c.String(maxLength: 255, unicode: false),
                        TransactionDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserPurchaseHistoryId);
            
        }
        
        public override void Down()
        {
            DropTable("Enterprise.tblUserPurchaseHistory");
        }
    }
}
