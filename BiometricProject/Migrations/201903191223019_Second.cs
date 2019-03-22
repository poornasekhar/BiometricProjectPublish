namespace BiometricProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDetailsModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AadharNumber = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(),
                        PhoneNumber = c.Int(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        OTP = c.Int(nullable: false),
                        OTPGenertedTime = c.DateTime(nullable: false),
                        BiometricImage = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Biometric.Bio_UserDetails", "ValidateAadhar_OTP", c => c.Int(nullable: false));
            AddColumn("Biometric.Bio_UserDetails", "ValidateAadhar_OTPGenertedTime", c => c.DateTime(nullable: false));
            AddColumn("Biometric.Bio_UserDetails", "IsVoted", c => c.Boolean(nullable: false));
            AddColumn("Biometric.Bio_UserDetails", "ValidateVoting_OTP", c => c.Int(nullable: false));
            AddColumn("Biometric.Bio_UserDetails", "ValidateVoting_OTPGenertedTime", c => c.DateTime(nullable: false));
            DropColumn("Biometric.Bio_UserDetails", "OTP");
            DropColumn("Biometric.Bio_UserDetails", "OTPGenertedTime");
        }
        
        public override void Down()
        {
            AddColumn("Biometric.Bio_UserDetails", "OTPGenertedTime", c => c.DateTime(nullable: false));
            AddColumn("Biometric.Bio_UserDetails", "OTP", c => c.Int(nullable: false));
            DropColumn("Biometric.Bio_UserDetails", "ValidateVoting_OTPGenertedTime");
            DropColumn("Biometric.Bio_UserDetails", "ValidateVoting_OTP");
            DropColumn("Biometric.Bio_UserDetails", "IsVoted");
            DropColumn("Biometric.Bio_UserDetails", "ValidateAadhar_OTPGenertedTime");
            DropColumn("Biometric.Bio_UserDetails", "ValidateAadhar_OTP");
            DropTable("dbo.UserDetailsModels");
        }
    }
}
