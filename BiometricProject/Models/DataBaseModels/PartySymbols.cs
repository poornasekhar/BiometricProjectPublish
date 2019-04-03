using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiometricProject.Models.DataBaseModels
{
    [Table("PartySymbols", Schema = "Biometric")]
    public class PartySymbols
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PartyName { get; set; }
        public byte[] PartySymbolImage { get; set; }
        public string PartyType { get; set; }
        public string PartyChief { get; set; }
    }
}