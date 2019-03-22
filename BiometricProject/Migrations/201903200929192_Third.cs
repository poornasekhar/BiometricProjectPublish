namespace BiometricProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Third : DbMigration
    {
        public override void Up()
        {
            DropIndex("Biometric.Bio_AadharDetails", new[] { "AadharNumber" });
            AddColumn("Biometric.Bio_AadharDetails", "Name", c => c.String());
            AddColumn("Biometric.Bio_AadharDetails", "OTPGeneratedTime", c => c.DateTime(nullable: false));
            AlterColumn("Biometric.Bio_AadharDetails", "AadharNumber", c => c.Long(nullable: false));
            AlterColumn("Biometric.Bio_UserDetails", "AadharNumber", c => c.Long(nullable: false));
            AlterColumn("dbo.UserDetailsModels", "AadharNumber", c => c.Long(nullable: false));
            CreateIndex("Biometric.Bio_AadharDetails", "AadharNumber", unique: true);
            DropColumn("Biometric.Bio_AadharDetails", "FirstName");
            DropColumn("Biometric.Bio_AadharDetails", "MiddleName");
            DropColumn("Biometric.Bio_AadharDetails", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("Biometric.Bio_AadharDetails", "LastName", c => c.String());
            AddColumn("Biometric.Bio_AadharDetails", "MiddleName", c => c.String());
            AddColumn("Biometric.Bio_AadharDetails", "FirstName", c => c.String());
            DropIndex("Biometric.Bio_AadharDetails", new[] { "AadharNumber" });
            AlterColumn("dbo.UserDetailsModels", "AadharNumber", c => c.Int(nullable: false));
            AlterColumn("Biometric.Bio_UserDetails", "AadharNumber", c => c.Int(nullable: false));
            AlterColumn("Biometric.Bio_AadharDetails", "AadharNumber", c => c.Int(nullable: false));
            DropColumn("Biometric.Bio_AadharDetails", "OTPGeneratedTime");
            DropColumn("Biometric.Bio_AadharDetails", "Name");
            CreateIndex("Biometric.Bio_AadharDetails", "AadharNumber", unique: true);
        }
    }
}
