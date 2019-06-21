using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiometricProject.Models.Models
{
    public class AadharDetailsModel
    {
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please enter Aadhar Number")]
        [MinLength(12,ErrorMessage ="Aadhar Number must be 12 digits only"),MaxLength(12, ErrorMessage = "Aadhar Number must be 12 digits only")]
        public string AadharNumber { get; set; }
        [Required(ErrorMessage = "Please enter Phone Number")]
        [Index(IsUnique = true)]
        [MinLength(10, ErrorMessage = "Phone Number must be 10 digits only"), MaxLength(10, ErrorMessage = "Phone Number must be 10 digits only")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter DOB")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please enter Assembly Constituency Id")]
        public string AssemblyConstituencyId { get; set; }
        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }
        public byte[] PhotoImage { get; set; }
        [Required(ErrorMessage = "Please upload Biometric Image")]
        public byte[] BiometricImage { get; set; }
        public bool IsEnable { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessage { get; set; }
    }
}