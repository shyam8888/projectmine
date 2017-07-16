using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Models;
using System.Globalization;
using LaboUSER.Controllers;
using System.IO;
using System.Net.Mail;

namespace LaboUSER.Areas.user.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /user/Register/
        private readonly LABOEntities _dataContext;
        public readonly SentEmailController _sendemail;
        public RegisterController()
        {
            _dataContext = new LABOEntities();
            _sendemail = new SentEmailController();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoadCity(int id)
        {
            var city = _dataContext.CityMasters.Where(s => s.StateID == id).ToList();
            var modeldata = city.Select(m => new SelectListItem()
            {
                Text = m.Name,
                Value = m.ID.ToString()
            }
               );
            return Json(modeldata, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Signup()
        {
            ViewBag.StateList = _dataContext.StateMasters.Where(s => s.CountryID == 38).ToList().Select(s => new SelectListItem { Text = s.Name, Value = s.ID.ToString() });
            return View();
        }
        [HttpPost]
        public ActionResult Signup(tbl_Users model, FormCollection fc, HttpPostedFileBase profilephoto, HttpPostedFileBase Gov_IssueId, HttpPostedFileBase PoliceVerification, HttpPostedFileBase EligibleWorkId)
        {
            string type = Convert.ToString(fc["type"]);
            if (type == "Employer")
            {
                tbl_Roles role = _dataContext.tbl_Roles.Where(s => s.RoleName == "Employee").FirstOrDefault();
                model.Fk_RoleId = role.Pk_RoleId;
            }
            if (type == "Client")
            {
                tbl_Roles role = _dataContext.tbl_Roles.Where(s => s.RoleName == "Client").FirstOrDefault();
                model.Fk_RoleId = role.Pk_RoleId;
            }
            model.Pk_UserId = Guid.NewGuid();
            model.IsActive = false;
            model.IsBlocked = false;
            model.CreatedDate = DateTime.Now;
            model.ModifiedDate = DateTime.Now;
            model.Fk_CountryId = 38;
            model.Country = "Canada";
            model.State = _dataContext.StateMasters.Where(s => s.ID == model.Fk_StateId).SingleOrDefault().Name;
            model.City = _dataContext.CityMasters.Where(s => s.ID == model.Fk_CityId).SingleOrDefault().Name;
            if (ModelState.IsValid)
            {
                if (profilephoto != null)
                {
                    var Image = Guid.NewGuid().ToString().Replace("-", "");
                    string extension = Path.GetExtension(profilephoto.FileName);
                    var path = Path.Combine(Server.MapPath("~/profile_photo/"), Image + extension);
                    profilephoto.SaveAs(path);
                    model.UserImage = Image + extension;
                }
                if (Gov_IssueId != null)
                {
                    var Image = Guid.NewGuid().ToString().Replace("-", "");
                    string extension = Path.GetExtension(Gov_IssueId.FileName);
                    var path = Path.Combine(Server.MapPath("~/user_doc/"), Image + extension);
                    Gov_IssueId.SaveAs(path);
                    model.Gov_IssueId = Image + extension;
                }
                if (PoliceVerification != null)
                {
                    var Image = Guid.NewGuid().ToString().Replace("-", "");
                    string extension = Path.GetExtension(PoliceVerification.FileName);
                    var path = Path.Combine(Server.MapPath("~/user_doc/"), Image + extension);
                    PoliceVerification.SaveAs(path);
                    model.PoliceVerification = Image + extension;
                }
                if (EligibleWorkId != null)
                {
                    var Image = Guid.NewGuid().ToString().Replace("-", "");
                    string extension = Path.GetExtension(EligibleWorkId.FileName);
                    var path = Path.Combine(Server.MapPath("~/user_doc/"), Image + extension);
                    EligibleWorkId.SaveAs(path);
                    model.EligibleWorkId = Image + extension;
                }
                _dataContext.tbl_Users.Add(model);
                _dataContext.SaveChanges();
            }
            else
            {
                ViewBag.StateList = _dataContext.StateMasters.Where(s => s.CountryID == 38).ToList().Select(s => new SelectListItem { Text = s.Name, Value = s.ID.ToString() });
                return View(model);
            }
            //For Send Mail...!!!
            MailMessage message = new MailMessage(
            "info@hardyhat.com", // From field
            model.EmailId, // Recipient field
            "Register Successfully", // Subject of the email message
            PopulateBody(model.FirstName+""+model.LastName) // Email message body
            );
            _sendemail.SendEmail(message);
            return RedirectToRoute("ThankYou");
        }
        public JsonResult IsEmail_Available(string EmailId)
        {
            bool isExist = _dataContext.tbl_Users.Where(s => s.EmailId.ToLower().Equals(EmailId.ToLower())).FirstOrDefault() != null;
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsContact_Available(string ContactNo)
        {
            bool isExist = _dataContext.tbl_Users.Where(s => s.ContactNo.Equals(ContactNo)).FirstOrDefault() != null;
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }
        private string PopulateBody(string user)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/" + "register.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{user}}", user);
            return body;
        }

    }
}
