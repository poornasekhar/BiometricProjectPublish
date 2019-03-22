using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiometricProject.Models.DataBaseModels
{
    [Table("Bio_UserDetails", Schema = "Biometric")]
    public class UserDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual AadharDetails AadharDetails { get; set; }
        [Required(ErrorMessage ="Enter 12 digit Aadhar Number")]
        [Range(100000000000, 999999999999, ErrorMessage = "Aadhar Number must be 12 digit only")]
        public long AadharNumber { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter Phone Number")]
        public Int64 PhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter Email Address")]
        public string EmailAddress { get; set; }
        public Nullable<int> ValidateAadhar_OTP { get; set; }
        public DateTime? ValidateAadhar_OTPGenertedTime { get; set; }
        public byte[] BiometricImage { get; set; }
        public bool IsVoted { get; set; }
        public Nullable<int> ValidateVoting_OTP { get; set; }
        public DateTime? ValidateVoting_OTPGenertedTime { get; set; }
    }
}