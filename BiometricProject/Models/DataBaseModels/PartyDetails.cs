using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiometricProject.Models.DataBaseModels
{
    [Table("Bio_PartyDetails",Schema = "Biometric")]
    public class PartyDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CandidateName { get; set; }
        public int AssemblyConstituencyId { get; set; }
        public string PartyName { get; set; }
        public byte[] PartySymbolImage { get; set; }
        public int VoteCount { get; set; }
        public bool IsEnable { get; set; }
    }
}