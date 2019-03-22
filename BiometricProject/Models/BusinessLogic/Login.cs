using BiometricProject.App_Data;
using BiometricProject.Models.DataBaseModels;
using BiometricProject.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BiometricProject.Models.BusinessLogic
{
    public class Login
    {
        BiometricDBContext biometricDBContext;

        public List<PartyDetails> LoginValidation(long aadharNumber,int otp)
        {
            List<PartyDetails> partyDetails = new List<PartyDetails>();
            using(biometricDBContext=new BiometricDBContext())
            {
                var userDetails = biometricDBContext.UserDetails.Where(i => i.AadharNumber == aadharNumber).FirstOrDefault();
                if (userDetails != null && userDetails.ValidateAadhar_OTP==otp && userDetails.ValidateAadhar_OTPGenertedTime.Value.AddMinutes(15)>DateTime.Now && userDetails.IsVoted==false)
                {
                    partyDetails = biometricDBContext.PartyDetails.Where(i => i.AssemblyConstituencyId == userDetails.AadharDetails.AssemblyConstituencyId).ToList();
                }
            }
            return partyDetails;
        }

        public string ValidateAadharDetails(UserDetailsModel userDetailsModel)
        {
            using (biometricDBContext = new BiometricDBContext())
            {
                Random random = new Random();
                var userAadharDetails = biometricDBContext.AadharDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber && i.IsEnable==true).FirstOrDefault();
                if (userAadharDetails.IsRegistered)
                {
                    return "user already registerd";
                }
                else if (userAadharDetails!=null && userAadharDetails.PhoneNumber==userDetailsModel.PhoneNumber && userAadharDetails.EmailAddress.ToLower()== userDetailsModel.EmailAddress.ToLower() && userAadharDetails.Name.ToLower()==userDetailsModel.Name.ToLower())
                {
                    userAadharDetails.BiometricImage = userAadharDetails.BiometricImage;
                    userAadharDetails.OTP = random.Next(1000, 9999);
                    userAadharDetails.OTPGeneratedTime = DateTime.Now;
                    if(SendEmail("OTP for online voting register","OTP is: "+ userAadharDetails.OTP+" valid for 15 minutes only", userAadharDetails.EmailAddress)== "Success")
                    {
                        biometricDBContext.SaveChanges();
                        return "true";
                    }
                }
            }
            return "false";
        }

        //OTP Validate and create user in database
        public bool ValidateOTP(UserDetailsModel userDetailsModel)
        {
            using (biometricDBContext = new BiometricDBContext())
            {
                try
                {
                    var userAadharDetails = biometricDBContext.AadharDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber && i.IsEnable == true).FirstOrDefault();
                    if (userAadharDetails != null && userAadharDetails.OTP == userDetailsModel.OTP && userAadharDetails.OTPGeneratedTime.Value.AddMinutes(15) > DateTime.Now)
                    {
                        UserDetails userDetails = new UserDetails()
                        {
                            //AadharDetails = userAadharDetails,
                            AadharNumber = userAadharDetails.AadharNumber,
                            EmailAddress = userAadharDetails.EmailAddress,
                            PhoneNumber = userAadharDetails.PhoneNumber,
                            IsVoted = false,
                            Password = userDetailsModel.Password,
                            BiometricImage = userAadharDetails.BiometricImage
                        };
                        biometricDBContext.UserDetails.Add(userDetails);
                        biometricDBContext.SaveChanges();
                        return true;
                    }
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        //Validate Login credentails and generate OTP for login
        public bool ValidateLoginDetails(UserDetailsModel userDetailsModel)
        {
            using (biometricDBContext = new BiometricDBContext())
            {
                var userDetails = biometricDBContext.UserDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber && i.Password == userDetailsModel.Password).FirstOrDefault();
                if (userDetails != null)
                {
                    Random random = new Random();
                    userDetails.ValidateVoting_OTP = random.Next(1000, 9999);
                    userDetails.ValidateVoting_OTPGenertedTime = DateTime.Now;
                    if (SendEmail("OTP for login into online voting", "OTP is: " + userDetails.ValidateVoting_OTP + " valid for 15 minutes only", userDetails.EmailAddress) == "Success")
                    {
                        biometricDBContext.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        //Validate OTP credentails for login
        public string ValidateOTPLoginDetails(UserDetailsModel userDetailsModel)
        {
            using (biometricDBContext = new BiometricDBContext())
            {
                var userDetails = biometricDBContext.UserDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber && i.Password == userDetailsModel.Password).FirstOrDefault();
                if (userDetails != null && userDetails.ValidateVoting_OTP== userDetailsModel.OTP && userDetails.ValidateVoting_OTPGenertedTime.Value.AddMinutes(5)>DateTime.Now && userDetails.IsVoted==false)
                {
                    return "true";
                }
                else if (userDetails.IsVoted)
                {
                    return "already voted";
                }
            }
            return "false";
        }

        //For sending OTP
        public string SendEmail(string Subject, string Body, string ToAddress)
        {
            string Result = "Empty";
            string UserName = Properties.Settings.Default.Email_UserName.ToString();
            string Password = Properties.Settings.Default.Email_Password.ToString();
            string SMTPServer = Properties.Settings.Default.Email_SMTPServer.ToString();
            int PortNo = Convert.ToInt32(Properties.Settings.Default.Email_PortNumber.ToString());
            bool IsSSL = Convert.ToBoolean(Properties.Settings.Default.Email_IsSSL.ToString());
            if (ToAddress == null || ToAddress.Length == 0)
            {
                return Result = "Email To Address Empty";
            }
            SmtpClient smtpClient = new SmtpClient(SMTPServer, PortNo);
            smtpClient.EnableSsl = IsSSL;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(UserName, Password);
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(UserName);
                message.Subject = Subject == null ? "" : Subject;
                message.Body = Body == string.Empty ? "" : Body;
                message.IsBodyHtml = true;
                message.To.Add(ToAddress);

                //message.To.Add(ToAddress);
                //if (CCAddress != null && CCAddress.Length > 0)
                //{
                //    foreach (string emailCc in CCAddress)
                //    {
                //        if (emailCc != "" && emailCc != null)
                //            message.CC.Add(emailCc);
                //    }
                //}
                try
                {
                    smtpClient.Send(message);
                    Result = "Success";
                }
                catch (Exception ex)
                {
                    Result = "Failed : " + ex.Message.ToString();
                }
            }
            return Result;
        }
    }
}