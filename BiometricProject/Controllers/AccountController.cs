using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        public JsonResult ValidateAadharDetails(UserDetailsModel userDetailsModel)
        {
            var userAadharDetails = biometricDBContext.AadharDetails.Where(i => i.AadharNumber == userDetailsModel.AadharNumber && i.IsEnable == true && i.IsRegistered == false).FirstOrDefault();
            var result = login.ValidateAadharDetails(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

         //OTP Validate and create user in database
        public JsonResult ValidateOTP(UserDetailsModel userDetailsModel)
        {
            var result = login.ValidateOTP(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Validate Login credentails and generate OTP for login
        public JsonResult ValidateLoginDetails(UserDetailsModel userDetailsModel)
        {
            var result = login.ValidateLoginDetails(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Validate Login credentails and generate OTP for login
        public JsonResult ValidateOTPLoginDetails(UserDetailsModel userDetailsModel)
        {
            var result = login.ValidateOTPLoginDetails(userDetailsModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        } 
        
        public ActionResult PartyDetails(int aadharNumber)
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
                    partyDetails = biometricDBContext.PartyDetails.Where(i => i.AssemblyConstituencyId == aadharDetails.AssemblyConstituencyId).ToList();
                    votingBaseModel.partyDetails = partyDetails;
                    votingBaseModel.userDetails = biometricDBContext.UserDetails.Where(i => i.AadharNumber == aadharNumber).FirstOrDefault();
                }
            }
            return View(votingBaseModel);
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
