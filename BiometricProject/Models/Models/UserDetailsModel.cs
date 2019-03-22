using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiometricProject.Models.Models
{
    public class UserDetailsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter 12 digit Aadhar Number")]
        [Range(100000000000, 999999999999, ErrorMessage = "Aadhar Number must be 12 digit only")]
        public long AadharNumber { get; set; }
        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Enter Phone Number")]
        public Int64 PhoneNumber { get; set; }
        [Required(ErrorMessage = "Enter Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Enter 4 digit OTP")]
        [Range(1000, 9999, ErrorMessage = "OTP must be 4 digit only")]
        public int OTP { get; set; }
        public DateTime OTPGenertedTime { get; set; }
        //[Required(ErrorMessage = "Upload Biometric")]
        public byte[] BiometricImage { get; set; }
    }
}