namespace SaveMyHome.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntitiesWithoutAttributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientProfiles", "FirstName", c => c.String());
            AlterColumn("dbo.ClientProfiles", "LastName", c => c.String());
            AlterColumn("dbo.Messages", "Text", c => c.String());
            AlterColumn("dbo.Problems", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Problems", "Name", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Messages", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.ClientProfiles", "LastName", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.ClientProfiles", "FirstName", c => c.String(nullable: false, maxLength: 15));
        }
    }
}
