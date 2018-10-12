namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedModelToUsePhotoId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Comments", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Name", c => c.String());
        }
    }
}
