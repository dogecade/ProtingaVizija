namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "idas", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "idas");
        }
    }
}
