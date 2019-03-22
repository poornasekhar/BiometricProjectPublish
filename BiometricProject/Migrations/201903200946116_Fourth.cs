namespace BiometricProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth : DbMigration
    {
        public override void Up()
        {
            DropIndex("Biometric.Bio_AadharDetails", new[] { "PhoneNumber" });
            AlterColumn("Biometric.Bio_AadharDetails", "PhoneNumber", c => c.Long(nullable: false));
            AlterColumn("Biometric.Bio_AadharDetails", "OTP", c => c.Int());
            AlterColumn("Biometric.Bio_UserDetails", "PhoneNumber", c => c.Long(nullable: false));
            AlterColumn("dbo.UserDetailsModels", "PhoneNumber", c => c.Long(nullable: false));
            CreateIndex("Biometric.Bio_AadharDetails", "PhoneNumber", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("Biometric.Bio_AadharDetails", new[] { "PhoneNumber" });
            AlterColumn("dbo.UserDetailsModels", "PhoneNumber", c => c.Int(nullable: false));
            AlterColumn("Biometric.Bio_UserDetails", "PhoneNumber", c => c.Int(nullable: false));
            AlterColumn("Biometric.Bio_AadharDetails", "OTP", c => c.Int(nullable: false));
            AlterColumn("Biometric.Bio_AadharDetails", "PhoneNumber", c => c.Int(nullable: false));
            CreateIndex("Biometric.Bio_AadharDetails", "PhoneNumber", unique: true);
        }
    }
}
