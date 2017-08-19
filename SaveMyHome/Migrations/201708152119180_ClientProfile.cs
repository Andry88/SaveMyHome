namespace SaveMyHome.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientProfile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ApartmentNumber", "dbo.Apartments");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ApartmentNumber" });
            CreateTable(
                "dbo.ClientProfiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 15),
                        LastName = c.String(nullable: false, maxLength: 15),
                        Age = c.Int(nullable: false),
                        SecondPhoneNumber = c.String(),
                        Skills = c.String(),
                        Hobbies = c.String(),
                        ImageData = c.Binary(),
                        ImageMimeType = c.String(),
                        ApartmentNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apartments", t => t.ApartmentNumber, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ApartmentNumber);
            
            AddColumn("dbo.Messages", "ClientProfile_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Messages", "ClientProfile_Id");
            AddForeignKey("dbo.Messages", "ClientProfile_Id", "dbo.ClientProfiles", "Id");
            DropColumn("dbo.Messages", "User_Id");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "SecondPhoneNumber");
            DropColumn("dbo.AspNetUsers", "Skills");
            DropColumn("dbo.AspNetUsers", "Hobbies");
            DropColumn("dbo.AspNetUsers", "ImageData");
            DropColumn("dbo.AspNetUsers", "ImageMimeType");
            DropColumn("dbo.AspNetUsers", "ApartmentNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ApartmentNumber", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "ImageMimeType", c => c.String());
            AddColumn("dbo.AspNetUsers", "ImageData", c => c.Binary());
            AddColumn("dbo.AspNetUsers", "Hobbies", c => c.String());
            AddColumn("dbo.AspNetUsers", "Skills", c => c.String());
            AddColumn("dbo.AspNetUsers", "SecondPhoneNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 15));
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 15));
            AddColumn("dbo.Messages", "User_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Messages", "ClientProfile_Id", "dbo.ClientProfiles");
            DropForeignKey("dbo.ClientProfiles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClientProfiles", "ApartmentNumber", "dbo.Apartments");
            DropIndex("dbo.Messages", new[] { "ClientProfile_Id" });
            DropIndex("dbo.ClientProfiles", new[] { "ApartmentNumber" });
            DropIndex("dbo.ClientProfiles", new[] { "Id" });
            DropColumn("dbo.Messages", "ClientProfile_Id");
            DropTable("dbo.ClientProfiles");
            CreateIndex("dbo.AspNetUsers", "ApartmentNumber");
            CreateIndex("dbo.Messages", "User_Id");
            AddForeignKey("dbo.Messages", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "ApartmentNumber", "dbo.Apartments", "Number", cascadeDelete: true);
        }
    }
}
