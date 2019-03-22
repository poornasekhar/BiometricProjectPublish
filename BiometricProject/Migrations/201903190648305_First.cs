namespace BiometricProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bio_AadharDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AadharNumber = c.Int(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        AssemblyConstituencyId = c.Int(nullable: false),
                        Address = c.String(),
                        PhotoImage = c.Binary(),
                        BiometricImage = c.Binary(),
                        OTP = c.Int(nullable: false),
                        IsEnable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.AadharNumber, unique: true)
                .Index(t => t.PhoneNumber, unique: true);
            
            CreateTable(
                "dbo.Bio_PartyDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CandidateName = c.String(),
                        AssemblyConstituencyId = c.Int(nullable: false),
                        PartyName = c.String(),
                        PartySymbolImage = c.Binary(),
                        VoteCount = c.Int(nullable: false),
                        IsEnable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bio_UserDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AadharNumber = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        OTP = c.Int(nullable: false),
                        OTPGenertedTime = c.DateTime(nullable: false),
                        BiometricImage = c.Binary(),
                        AadharDetails_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bio_AadharDetails", t => t.AadharDetails_Id)
                .Index(t => t.AadharDetails_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bio_UserDetails", "AadharDetails_Id", "dbo.Bio_AadharDetails");
            DropIndex("dbo.Bio_UserDetails", new[] { "AadharDetails_Id" });
            DropIndex("dbo.Bio_AadharDetails", new[] { "PhoneNumber" });
            DropIndex("dbo.Bio_AadharDetails", new[] { "AadharNumber" });
            DropTable("dbo.Bio_UserDetails");
            DropTable("dbo.Bio_PartyDetails");
            DropTable("dbo.Bio_AadharDetails");
        }
    }
}
