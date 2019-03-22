namespace BiometricProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Biometric.Bio_UserDetails", "ValidateAadhar_OTP", c => c.Int());
            AlterColumn("Biometric.Bio_UserDetails", "ValidateVoting_OTP", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("Biometric.Bio_UserDetails", "ValidateVoting_OTP", c => c.Int(nullable: false));
            AlterColumn("Biometric.Bio_UserDetails", "ValidateAadhar_OTP", c => c.Int(nullable: false));
        }
    }
}
