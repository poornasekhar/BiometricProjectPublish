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
            HttpPostedFileBase vendorPic = Request.Files["vendorPic"];
                    if (vendorPic != null && vendorPic.ContentLength > 0)
                    {
                        int length = vendorPic.ContentLength;
                        userDetailsModel.BiometricImage = new byte[length];
                        vendorPic.InputStream.Read(userDetailsModel.BiometricImage, 0, length);
                    }
            var userAadharDetails = biometricDBContext.AadharDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber && i.IsEnable == true && i.IsRegistered == false).FirstOrDefault();
            var result = login.ValidateAadharDetails(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[ValidateAntiForgeryToken]
        //OTP Validate and create user in database
        public JsonResult ValidateOTP(UserDetailsModel userDetailsModel)
        {
            var result = login.ValidateOTP(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[ValidateAntiForgeryToken]
        //Validate Login credentails and generate OTP for login
        public JsonResult ValidateLoginDetails(UserDetailsModel userDetailsModel,HttpPostedFileBase logo)
        {
            HttpPostedFileBase vendorPic = Request.Files["vendorPic"];
                    if (vendorPic != null && vendorPic.ContentLength > 0)
                    {
                        int length = vendorPic.ContentLength;
                        userDetailsModel.BiometricImage = new byte[length];
                        vendorPic.InputStream.Read(userDetailsModel.BiometricImage, 0, length);
                    }
            var result = login.ValidateLoginDetails(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[ValidateAntiForgeryToken]
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
                        if(partyRegisterModel.Id==0)
                        biometricDBContext.PartySymbols.Add(partySymbols);
                        else
                        {
                            var registeredPartyDetails = biometricDBContext.PartySymbols.Where(i => i.Id == partyRegisterModel.Id).FirstOrDefault();
                            biometricDBContext.Entry(registeredPartyDetails).CurrentValues.SetValues(partySymbols);
                        }
                        biometricDBContext.SaveChanges();
                    }
                }
            }
            catch(Exception e)
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

        // GET: Account
        public ActionResult Index()
        {
            return View(biometricDBContext.UserDetailsModels.ToList());
        }

        // GET: Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetailsModel userDetailsModel = biometricDBContext.UserDetailsModels.Find(id);
            if (userDetailsModel == null)
            {
                return HttpNotFound();
            }
            return View(userDetailsModel);
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AadharNumber,Password,ConfirmPassword,PhoneNumber,EmailAddress,OTP,OTPGenertedTime,BiometricImage")] UserDetailsModel userDetailsModel)
        {
            if (ModelState.IsValid)
            {
                biometricDBContext.UserDetailsModels.Add(userDetailsModel);
                biometricDBContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDetailsModel);
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetailsModel userDetailsModel = biometricDBContext.UserDetailsModels.Find(id);
            if (userDetailsModel == null)
            {
                return HttpNotFound();
            }
            return View(userDetailsModel);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AadharNumber,Password,ConfirmPassword,PhoneNumber,EmailAddress,OTP,OTPGenertedTime,BiometricImage")] UserDetailsModel userDetailsModel)
        {
            if (ModelState.IsValid)
            {
                biometricDBContext.Entry(userDetailsModel).State = EntityState.Modified;
                biometricDBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDetailsModel);
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetailsModel userDetailsModel = biometricDBContext.UserDetailsModels.Find(id);
            if (userDetailsModel == null)
            {
                return HttpNotFound();
            }
            return View(userDetailsModel);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserDetailsModel userDetailsModel = biometricDBContext.UserDetailsModels.Find(id);
            biometricDBContext.UserDetailsModels.Remove(userDetailsModel);
            biometricDBContext.SaveChanges();
            return RedirectToAction("Index");
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
