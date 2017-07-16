using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Models;
using LaboUSER.Areas.user.Filters;
using System.IO;
using System.Net.Mail;
using LaboUSER.Controllers;
namespace LaboUSER.Areas.user.Controllers
{
    [SessionCheckFilter]
    public class JobController : Controller
    {
        //
        // GET: /user/Job/
        private readonly LABOEntities _dataContext;
        public readonly SentEmailController _sendemail;
        public JobController()
        {
            _dataContext = new LABOEntities();
            _sendemail = new SentEmailController();
        }
        public ActionResult createjob()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "IT", Value = "IT" });
            list.Add(new SelectListItem { Text = "Hardware", Value = "Hardware" });
            list.Add(new SelectListItem { Text = "Painting", Value = "Painting" });
            var companyfee = _dataContext.tbl_Setting.SingleOrDefault();
            ViewBag.CompanyFee = companyfee.CompanyFee;
            ViewBag.JobCategoryList = list;
            return View();
        }
        [HttpPost]
        public ActionResult createJob(tbl_Jobs job)
        {
            if (ModelState.IsValid)
            {

                var companyfee = _dataContext.tbl_Setting.SingleOrDefault();
                job.CompanyFee = companyfee.CompanyFee;
                job.ClientUserId = clsSession.UserID;
                job.IsActive = true;
                job.UniqueJobId = Guid.NewGuid().ToString().Split('-')[0];
                job.ClientFeePaymentStatus = "Pending";
                job.JobStatus = "Pending";
                job.CreatedDate = DateTime.Now;
                _dataContext.tbl_Jobs.Add(job);
                _dataContext.SaveChanges();
            }
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "IT", Value = "IT" });
            list.Add(new SelectListItem { Text = "Hardware", Value = "Hardware" });
            list.Add(new SelectListItem { Text = "Painting", Value = "Painting" });

            ViewBag.JobCategoryList = list;
            return View(job);
        }
        public ActionResult myjobs()
        {
            Guid userid = clsSession.UserID;
            List<tbl_Jobs> lstJobs = new List<tbl_Jobs>();
            if (clsSession.RoleName == "Admin")
            {
                lstJobs = _dataContext.tbl_Jobs.ToList();
            }
            else
            {
                lstJobs = _dataContext.tbl_Jobs.Where(x => x.ClientUserId == userid && x.IsActive == true).ToList();
            }

            return View("~/Areas/user/Views/Job/MyJobs.cshtml", lstJobs);
        }
        public ActionResult View(int id)
        {
            tbl_Jobs job = _dataContext.tbl_Jobs.Where(x => x.Pk_JobId == id).FirstOrDefault();
            return View("~/Areas/user/Views/Job/ViewJob.cshtml", job);

        }
        public ActionResult jobsetting(int id)
        {
            var roleId = _dataContext.tbl_Roles.Where(s => s.RoleName == "Employee").FirstOrDefault();
            var userDetails = _dataContext.GET_JOB_EMPLOYEE(id, clsSession.UserID).Where(s => s.Fk_CityId == clsSession.CityID && s.Pk_UserId!=clsSession.UserID && s.Fk_RoleId == roleId.Pk_RoleId).ToList();
            TempData["JobID"] = id;
            return View(userDetails);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult searchEmployeeForJob(string searchvalue)
        {
            var userDetails = new List<GET_JOB_EMPLOYEE_Result>();
            if (searchvalue != "")
            {
                var roleId = _dataContext.tbl_Roles.Where(s => s.RoleName == "Employee").FirstOrDefault();
               
                userDetails = _dataContext.GET_JOB_EMPLOYEE(Convert.ToInt32(TempData.Peek("JobID")),clsSession.UserID).ToList().Where(s => s.City.ToLower().Contains(searchvalue.ToLower()) || s.ZipCode.ToLower().Contains(searchvalue.ToLower()) && s.Pk_UserId != clsSession.UserID && (s.Fk_RoleId == roleId.Pk_RoleId)).ToList();
            }
            else
            {
                var roleId = _dataContext.tbl_Roles.Where(s => s.RoleName == "Employee").FirstOrDefault();
                userDetails = _dataContext.GET_JOB_EMPLOYEE(Convert.ToInt32(TempData.Peek("JobID")), clsSession.UserID).ToList().Where(s => s.Fk_CityId == clsSession.CityID && s.Pk_UserId != clsSession.UserID && s.Fk_RoleId == roleId.Pk_RoleId).ToList();
            }
            return PartialView("_jobemployeelist", userDetails);
        }
        public ActionResult invitejob(Guid userid)
        {
            Int32 jobid = Convert.ToInt32(TempData.Peek("JobID"));
            tbl_JobEmployees jobemp = new tbl_JobEmployees();
            jobemp.Fk_JobId = jobid;
            jobemp.fromUserId = clsSession.UserID;
            jobemp.fromUserName = clsSession.UserName;
            jobemp.toUserId = userid;
            jobemp.EmplyeeFeePaymentStatus = "invitation sent";
            jobemp.CreatedDate = DateTime.Now;
            jobemp.IsActive = true;
            _dataContext.tbl_JobEmployees.Add(jobemp);
            _dataContext.SaveChanges();

            //Sent Email to employee to introduce about job detail...!!!
            //Get send user details...!!!
            tbl_Users touserdetail = _dataContext.tbl_Users.Where(s => s.Pk_UserId == userid).FirstOrDefault();
            //Get Job Detail...!!!
            tbl_Jobs job = _dataContext.tbl_Jobs.Where(s => s.Pk_JobId == jobid).FirstOrDefault();
            MailMessage message = new MailMessage(
           "info@hardyhat.com", // From field
           touserdetail.EmailId, // Recipient field
           "Job Request Sent by Client", // Subject of the email message
           PopulateBody(clsSession.UserName, job.JobTitle,job.JobCategory,job.JobLocation,"$"+job.Amount.ToString(),job.JobDescription) // Email message body
           );
            _sendemail.SendEmail(message);
            return PartialView("_invitationsentsuccess");
        }
        public ActionResult invitecandid()
        {
            Int32 jobid = Convert.ToInt32(TempData.Peek("JobID"));
            var userDetails = _dataContext.GET_JOB_EMPLOYEE(jobid, clsSession.UserID).ToList().Where(s => s.Fk_JobId == jobid && s.EmplyeeFeePaymentStatus == "invitation sent").ToList();
            return PartialView("_invitedcandid", userDetails);
        }
        public ActionResult hiredcandid()
        {
            Int32 jobid = Convert.ToInt32(TempData.Peek("JobID"));
            var userDetails = _dataContext.GET_JOB_EMPLOYEE(jobid, clsSession.UserID).ToList().Where(s => s.Fk_JobId == jobid && s.Pk_UserId!=clsSession.UserID && s.EmplyeeFeePaymentStatus == "Approved").ToList();
            return PartialView("_hiredcandid", userDetails);
        }
        private string PopulateBody(string clientname,string jobtitle,string jobcategory,string joblocation,string amountpay,string jobdescription)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/" + "jobrequest.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{client_name}}", clientname);
            body = body.Replace("{{job_title}}", jobtitle);
            body = body.Replace("{{job_category}}", jobcategory);
            body = body.Replace("{{job_location}}", joblocation);
            body = body.Replace("{{amount_pay}}", amountpay);
            body = body.Replace("{{job_description}}", jobdescription);
            return body;
        }
    }
}
