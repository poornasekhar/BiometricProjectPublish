namespace BiometricProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Five : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Biometric.PartySymbols",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartyName = c.String(),
                        PartySymbolImage = c.Binary(),
                        PartyType = c.String(),
                        PartyChief = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Biometric.Bio_PartyDetails", "PartySymbols_Id", c => c.Int());
            CreateIndex("Biometric.Bio_PartyDetails", "PartySymbols_Id");
            AddForeignKey("Biometric.Bio_PartyDetails", "PartySymbols_Id", "Biometric.PartySymbols", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Biometric.Bio_PartyDetails", "PartySymbols_Id", "Biometric.PartySymbols");
            DropIndex("Biometric.Bio_PartyDetails", new[] { "PartySymbols_Id" });
            DropColumn("Biometric.Bio_PartyDetails", "PartySymbols_Id");
            DropTable("Biometric.PartySymbols");
        }
    }
}
