using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiometricProject.Models.DataBaseModels
{
    [Table("Bio_AadharDetails",Schema ="Biometric")]
    public class AadharDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [Range(100000000000, 999999999999,ErrorMessage ="Aadhar Number must be 12 digit only")]
        public long AadharNumber { get; set; }
        [Index(IsUnique = true)]
        public Int64 PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AssemblyConstituencyId{ get; set; }
        public string Address { get; set; }
        public byte[] PhotoImage { get; set; }
        public byte[] BiometricImage { get; set; }
        public Nullable<int> OTP { get; set; }
        public DateTime? OTPGeneratedTime { get; set; }
        public bool IsEnable { get; set; }
        public bool IsRegistered { get; set; }
    }
}