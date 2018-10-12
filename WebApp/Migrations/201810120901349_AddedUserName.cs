namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments1", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments1", "UserName");
        }
    }
}
