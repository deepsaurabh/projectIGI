namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckoutTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Enterprise.tblCheckOutAddress",
                c => new
                    {
                        CheckOutAddressId = c.Long(nullable: false, identity: true),
                        UserName = c.String(maxLength: 255, unicode: false),
                        CompleteAddress = c.String(maxLength: 1000, unicode: false),
                        PhoneNumber = c.String(maxLength: 25, unicode: false),
                        State = c.String(maxLength: 255, unicode: false),
                        City = c.String(maxLength: 255, unicode: false),
                        PinCode = c.String(maxLength: 25, unicode: false),
                        EmailAddress = c.String(maxLength: 255, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CheckOutAddressId);
            
        }
        
        public override void Down()
        {
            DropTable("Enterprise.tblCheckOutAddress");
        }
    }
}
