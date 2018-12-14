namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class phoneNumberis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "phoneNumberis", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "phoneNumberis");
        }
    }
}
