namespace BiometricProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Biometric.Bio_AadharDetails", "IsRegistered", c => c.Boolean(nullable: false));
            AlterColumn("Biometric.Bio_AadharDetails", "OTPGeneratedTime", c => c.DateTime());
            AlterColumn("Biometric.Bio_UserDetails", "ValidateAadhar_OTPGenertedTime", c => c.DateTime());
            AlterColumn("Biometric.Bio_UserDetails", "ValidateVoting_OTPGenertedTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("Biometric.Bio_UserDetails", "ValidateVoting_OTPGenertedTime", c => c.DateTime(nullable: false));
            AlterColumn("Biometric.Bio_UserDetails", "ValidateAadhar_OTPGenertedTime", c => c.DateTime(nullable: false));
            AlterColumn("Biometric.Bio_AadharDetails", "OTPGeneratedTime", c => c.DateTime(nullable: false));
            DropColumn("Biometric.Bio_AadharDetails", "IsRegistered");
        }
    }
}
