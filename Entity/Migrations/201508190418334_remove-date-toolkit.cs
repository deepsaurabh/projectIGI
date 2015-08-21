namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedatetoolkit : DbMigration
    {
        public override void Up()
        {
            DropColumn("Enterprise.tblToolkit", "StartDate");
            DropColumn("Enterprise.tblToolkit", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("Enterprise.tblToolkit", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("Enterprise.tblToolkit", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
