namespace BiometricProject.App_Data
{
    using BiometricProject.Models.DataBaseModels;
    using System.Data.Entity;

    public class BiometricDBContext : DbContext
    {
        public BiometricDBContext() : base()
        {
            //BiometricDBContext biometricDBContext = new BiometricDBContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AadharDetails> AadharDetails { get; set; }
        public DbSet<PartyDetails> PartyDetails { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }

        public System.Data.Entity.DbSet<BiometricProject.Models.Models.UserDetailsModel> UserDetailsModels { get; set; }
    }
}
