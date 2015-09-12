namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_name_with_address : DbMigration
    {
        public override void Up()
        {
            AddColumn("Enterprise.tblCheckOutAddress", "Name", c => c.String(maxLength: 255, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Enterprise.tblCheckOutAddress", "Name");
        }
    }
}
