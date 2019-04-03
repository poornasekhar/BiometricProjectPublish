using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiometricProject.Models.Models
{
    public class PartyRegisterModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter Party Name")]
        public string PartyName { get; set; }
        [Required(ErrorMessage = "Please select Party Logo")]
        public byte[] PartySymbolImage { get; set; }
        public string PartyType { get; set; }
        [Required(ErrorMessage = "Please enter Party Chief Name")]
        public string PartyChief { get; set; }
    }
}