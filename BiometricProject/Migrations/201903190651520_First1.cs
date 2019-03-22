namespace BiometricProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First1 : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Bio_AadharDetails", newSchema: "Biometric");
            MoveTable(name: "dbo.Bio_PartyDetails", newSchema: "Biometric");
            MoveTable(name: "dbo.Bio_UserDetails", newSchema: "Biometric");
        }
        
        public override void Down()
        {
            MoveTable(name: "Biometric.Bio_UserDetails", newSchema: "dbo");
            MoveTable(name: "Biometric.Bio_PartyDetails", newSchema: "dbo");
            MoveTable(name: "Biometric.Bio_AadharDetails", newSchema: "dbo");
        }
    }
}
