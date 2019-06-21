using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BiometricProject.App_Data;
using BiometricProject.Models.BusinessLogic;
using BiometricProject.Models.DataBaseModels;
using BiometricProject.Models.Models;

namespace BiometricProject.Controllers
{
    public class AccountController : Controller
    {
        Login login = new Login();
        private BiometricDBContext biometricDBContext = new BiometricDBContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserDetailsModel userDetailsModel)
        {
            var partiesList = login.LoginValidation(userDetailsModel.AadharNumber, userDetailsModel.OTP);
            if (partiesList.Count > 0)
            {

            }
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserDetailsModel userDetailsModel)
        {

            return View();
        }

        //[ValidateAntiForgeryToken]
        public JsonResult ValidateAadharDetails(UserDetailsModel userDetailsModel,HttpPostedFileBase logo)
        {
            string purpose = "new registration"; //For differenciate between reset password and new registration
            HttpPostedFileBase vendorPic = Request.Files["vendorPic"];
                    if (vendorPic != null && vendorPic.ContentLength > 0)
                    {
                        int length = vendorPic.ContentLength;
                        userDetailsModel.BiometricImage = new byte[length];
                        vendorPic.InputStream.Read(userDetailsModel.BiometricImage, 0, length);
                    }
            var userAadharDetails = biometricDBContext.AadharDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber && i.IsEnable == true && i.IsRegistered == false).FirstOrDefault();
            var result = login.ValidateAadharDetails(userDetailsModel, purpose);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[ValidateAntiForgeryToken]
        //OTP Validate and create user in database
        public JsonResult ValidateOTP(UserDetailsModel userDetailsModel, HttpPostedFileBase logo)
        {
            HttpPostedFileBase vendorPic = Request.Files["vendorPic"];
            if (vendorPic != null && vendorPic.ContentLength > 0)
            {
                int length = vendorPic.ContentLength;
                userDetailsModel.BiometricImage = new byte[length];
                vendorPic.InputStream.Read(userDetailsModel.BiometricImage, 0, length);
            }
            var result = login.ValidateOTP(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        //Validate Login credentails and generate OTP for login
        public JsonResult ValidateLoginDetails(/*UserDetailsModel userDetailsModel,HttpPostedFileBase logo*/)
        {
            HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];
                int length = file.ContentLength;
                string fileName = file.FileName;
                byte[] biometricImage = new byte[length];
                file.InputStream.Read(biometricImage, 0, length);
                long aadharNumber = Convert.ToInt64(Request["AadharNumber"]);
                string password =(Request["Password"]);
                FileStream bioImage = new FileStream(Server.MapPath("~/images/" + fileName /*+ ".jpg"*/), FileMode.Create, FileAccess.Write);
            bioImage.Write(biometricImage, 0, biometricImage.Length);
            string uploadedBiometricImagePath = Server.MapPath("~/images/" + fileName);
            bioImage.Close();

            var existingAadharDetails = biometricDBContext.AadharDetails.Where(i => i.AadharNumber == aadharNumber).FirstOrDefault();
            if (existingAadharDetails == null)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            else
            {
                FileStream biometricImag = new FileStream(Server.MapPath("~/images/" + existingAadharDetails.AadharNumber + ".jpg"), FileMode.Create, FileAccess.Write);
                biometricImag.Write(existingAadharDetails.BiometricImage, 0, existingAadharDetails.BiometricImage.Length);
                string savedBiometricImagepath = Server.MapPath("~/images/" + existingAadharDetails.AadharNumber + ".jpg");
                biometricImag.Close();
                System.IO.File.Delete(uploadedBiometricImagePath);
                System.IO.File.Delete(savedBiometricImagepath);
            }
            UserDetailsModel userDetailsModel = new UserDetailsModel();
            userDetailsModel.AadharNumber = aadharNumber;
            userDetailsModel.Password = password;
            userDetailsModel.BiometricImage = biometricImage;
            var result = login.ValidateLoginDetails(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //Validate Login credentails and generate OTP for login
        public JsonResult ValidateOTPLoginDetails(UserDetailsModel userDetailsModel)
        {
            var result = login.ValidateOTPLoginDetails(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        } 
        
        //[ValidateAntiForgeryToken]
        public ActionResult PartyDetails(long aadharNumber,int OTP)
        {
            VotingBaseModel votingBaseModel = new VotingBaseModel()
            {
            };
            List<PartyDetails> partyDetails = new List<PartyDetails>();
            if (aadharNumber > 0)
            {
                using (biometricDBContext)
                {
                    var aadharDetails = biometricDBContext.AadharDetails.Where(i => i.AadharNumber == aadharNumber).FirstOrDefault();
                    partyDetails = biometricDBContext.PartyDetails.Where(i => i.AssemblyConstituencyId == aadharDetails.AssemblyConstituencyId || i.AssemblyConstituencyId==1).ToList();
                    votingBaseModel.partyDetails = partyDetails;
                    votingBaseModel.userDetails = biometricDBContext.UserDetails.Where(i => i.AadharNumber == aadharNumber).FirstOrDefault();
                    if (votingBaseModel.userDetails.IsVoted && votingBaseModel.userDetails.ValidateVoting_OTP!=OTP && votingBaseModel.userDetails.ValidateVoting_OTPGenertedTime.Value.AddMinutes(5)<DateTime.Now)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    foreach (var item in partyDetails)
                    {
                        if (item.PartySymbols.PartySymbolImage != null && item.PartySymbols.PartySymbolImage.Length > 0)
                        {
                            string imreBase64Data = Convert.ToBase64String(item.PartySymbols.PartySymbolImage);
                            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                            string fileName = item.PartySymbols.Id.ToString() + "-partysymbol";
                            if (!System.IO.File.Exists(Server.MapPath("~/images/" + fileName + ".jpg")))
                            {
                                FileStream partyImage = new FileStream(Server.MapPath("~/images/" + fileName + ".jpg"), FileMode.Create, FileAccess.Write);
                                partyImage.Write(item.PartySymbols.PartySymbolImage, 0, item.PartySymbols.PartySymbolImage.Length);
                                partyImage.Close();
                            }
                        }
                    }
                }
            }
            return View(votingBaseModel);
        }

        //[ValidateAntiForgeryToken]
        public ActionResult RegisterParty()
        {
            PartyRegisterModel partyRegisterModel = new PartyRegisterModel();
            return View(partyRegisterModel);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RegisterParty(PartyRegisterModel partyRegisterModel, HttpPostedFileBase logo)
        {
            HttpPostedFileBase vendorPic = Request.Files["vendorPic"];
            try
            {
                if (vendorPic != null && vendorPic.ContentLength > 0)
                {
                    int length = vendorPic.ContentLength;
                    partyRegisterModel.PartySymbolImage = new byte[length];
                    vendorPic.InputStream.Read(partyRegisterModel.PartySymbolImage, 0, length);
                    using (biometricDBContext)
                    {
                        PartySymbols partySymbols = new PartySymbols()
                        {
                            Id= partyRegisterModel.Id,
                            PartyName = partyRegisterModel.PartyName,
                            PartyChief = partyRegisterModel.PartyChief,
                            PartyType = partyRegisterModel.PartyType,
                            PartySymbolImage = partyRegisterModel.PartySymbolImage,
                        };
                        if (partyRegisterModel.Id == 0)
                        {
                            biometricDBContext.PartySymbols.Add(partySymbols);
                        }
                        else
                        {
                            var registeredPartyDetails = biometricDBContext.PartySymbols.Where(i => i.Id == partyRegisterModel.Id).FirstOrDefault();
                            biometricDBContext.Entry(registeredPartyDetails).CurrentValues.SetValues(partySymbols);
                        }
                        biometricDBContext.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return RedirectToAction("RegisterParty");
        }

        //[ValidateAntiForgeryToken]
        public ActionResult VoteDetails(VotingBaseModel votingBaseModel)
        {
            var votingMessage = login.SumbitVote(votingBaseModel);
            return Json(votingMessage, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResetPassword(long aadharNumber)
        {
            return View();
        }

        //[ValidateAntiForgeryToken]
        public JsonResult ValidateAadharDetailsForResetPassword(UserDetailsModel userDetailsModel)
        {
            string purpose = "reset password"; //For differenciate between reset password and new registration
            if(!biometricDBContext.AadharDetails.Any(a=>a.AadharNumber== userDetailsModel.AadharNumber))
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = login.ValidateAadharDetails(userDetailsModel, purpose);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //[ValidateAntiForgeryToken]
        public JsonResult ResetUserPassword(UserDetailsModel userDetailsModel)
        {
            var userDetails = biometricDBContext.UserDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber).FirstOrDefault();
            var aadharDetails= biometricDBContext.AadharDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber).FirstOrDefault();
            if (userDetails != null && aadharDetails.OTP==userDetailsModel.OTP && aadharDetails.OTPGeneratedTime.Value.AddMinutes(5)>=DateTime.Now)
            {
                userDetails.Password = userDetailsModel.Password;
                biometricDBContext.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RegisterAadhar()
        {
            AadharDetailsModel aadharDetails = new AadharDetailsModel();
            if (TempData["AadharSaveStatus"] != null)
            {
                aadharDetails.ReturnMessageType = TempData["AadharSaveStatus"].ToString();
                aadharDetails.ReturnMessage = TempData["AadharSaveStatusMessage"].ToString();
            }
            return View(aadharDetails);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RegisterAadhar(AadharDetailsModel aadharDetails, HttpPostedFileBase logo)
        {
            HttpPostedFileBase vendorPic = Request.Files["vendorPic"];
            try
            {
                if (vendorPic != null && vendorPic.ContentLength > 0)
                {
                    int length = vendorPic.ContentLength;
                    aadharDetails.BiometricImage = new byte[length];
                    vendorPic.InputStream.Read(aadharDetails.BiometricImage, 0, length);
                    using (biometricDBContext)
                    {
                        AadharDetails aadharDetailsObject = new AadharDetails()
                        {
                            Id = aadharDetails.Id,
                            Name = aadharDetails.Name,
                            AadharNumber = Convert.ToInt64(aadharDetails.AadharNumber),
                            BiometricImage= aadharDetails.BiometricImage,
                            DateOfBirth=aadharDetails.DateOfBirth,
                            Address=aadharDetails.Address,
                            AssemblyConstituencyId=Convert.ToInt32(aadharDetails.AssemblyConstituencyId),
                            PhoneNumber= Convert.ToInt64(aadharDetails.PhoneNumber),
                            EmailAddress=aadharDetails.EmailAddress,
                            IsEnable=true
                        };
                        if (aadharDetails.Id == 0)
                        {
                            biometricDBContext.AadharDetails.Add(aadharDetailsObject);
                        }
                        else
                        {
                            var exixtingAadharDetails = biometricDBContext.AadharDetails.Where(i => i.Id == aadharDetails.Id).FirstOrDefault();
                            biometricDBContext.Entry(exixtingAadharDetails).CurrentValues.SetValues(aadharDetails);
                        }
                        biometricDBContext.SaveChanges();
                        TempData["AadharSaveStatus"] = "Success";
                        TempData["AadharSaveStatusMessage"] = "Aadhar details saved successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AadharSaveStatus"] = "Failure";
                TempData["AadharSaveStatusMessage"] = ex.Message + " " + ex.InnerException.Message;
            }
            return RedirectToAction("RegisterAadhar");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                biometricDBContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
