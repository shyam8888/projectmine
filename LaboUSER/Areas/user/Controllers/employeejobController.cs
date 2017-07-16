using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Models;
using LaboUSER.Areas.user.Filters;
using LaboUSER.Areas.user.Models;
using LaboUSER.Controllers;
using System.IO;
using System.Net.Mail;
using LaboUSER.Areas.user.Models;
namespace LaboUSER.Areas.user.Controllers
{
    [SessionCheckFilter]
    public class employeejobController : Controller
    {
        private readonly LABOEntities _dataContext;
        public readonly SentEmailController _sendemail;
        public DataEncryptor keys = new DataEncryptor();
        public employeejobController()
        {
            _dataContext = new LABOEntities();
            _sendemail = new SentEmailController();
        }

        public ActionResult jobproposals()
        {
            var userDetails = _dataContext.GET_JOB_EMPLOYEE(0, clsSession.UserID).ToList().Where((s => s.EmplyeeFeePaymentStatus == "invitation sent" || s.EmplyeeFeePaymentStatus == "Approved" || s.EmplyeeFeePaymentStatus == "Rejected" && s.toUserId == clsSession.UserID)).ToList();
            return View(userDetails);
        }
        public ActionResult clientjobproposal()
        {
            var userDetails = _dataContext.GET_JOB_EMPLOYEE(0, clsSession.UserID).Where(s => s.EmplyeeFeePaymentStatus == "invitation sent" || s.EmplyeeFeePaymentStatus == "employee requested" || s.EmplyeeFeePaymentStatus == "Approved" || s.EmplyeeFeePaymentStatus == "Rejected" && s.toUserId == clsSession.UserID).ToList();
            return View(userDetails);
        }
        public ActionResult viewjob(Int32 id)
        {
            var jobDetails = _dataContext.tbl_Jobs.Where(s => s.Pk_JobId == id).FirstOrDefault();
            return View(jobDetails);
        }
        public ActionResult jobrequested()
        {
            var userDetails = _dataContext.GET_JOB_EMPLOYEE(0, clsSession.UserID).Where(s => s.EmplyeeFeePaymentStatus == "employee requested" || s.EmplyeeFeePaymentStatus == "Approved" && s.fromUserId == clsSession.UserID).ToList();
            return View(userDetails);
        }
        public ActionResult approve(Int32 id)
        {
            tbl_JobEmployees jobstatus = _dataContext.tbl_JobEmployees.Where(s => s.Fk_JobId == id).FirstOrDefault();
            jobstatus.EmplyeeFeePaymentStatus = "Approved";
            _dataContext.SaveChanges();
            //For Change Job Status...!!!
            tbl_Jobs job = _dataContext.tbl_Jobs.Where(s => s.Pk_JobId == id).FirstOrDefault();
            //Sent Email to employee to introduce about job detail...!!!
            //Get send user details...!!!
            tbl_Users touserdetail = _dataContext.tbl_Users.Where(s => s.Pk_UserId == clsSession.UserID).FirstOrDefault();
            //Get Job Detail...!!!
            string userid = keys.EncryptString(clsSession.UserID.ToString());
            string jobid = keys.EncryptString(id.ToString());
            string paymenturl = "http://hardyhat.com/user/payment/checkout?userid=" + userid + "&jobid=" + jobid;
            MailMessage message = new MailMessage(
           "info@hardyhat.com", // From field
           touserdetail.EmailId, // Recipient field
           "Pending For Payment Approval", // Subject of the email message
           PopulateBody(clsSession.UserName, job.JobTitle, job.JobCategory, job.JobLocation, "$" + job.Amount.ToString(), job.JobDescription, false, paymenturl, true, false) // Email message body
           );
            _sendemail.SendEmail(message);
            List<tbl_JobEmployees> jobemployeedata = _dataContext.tbl_JobEmployees.Where(s => s.Fk_JobId == id && s.EmplyeeFeePaymentStatus == "Approved").ToList();
            if (jobemployeedata.Count >= job.NoOfEmployeeNeeded)
            {
                job.JobStatus = "Approved";
                _dataContext.SaveChanges();
                //Sent Email to employee to introduce about job detail...!!!
                //Get send user details...!!!
                tbl_Users touserdetailClient = _dataContext.tbl_Users.Where(s => s.Pk_UserId == jobstatus.fromUserId).FirstOrDefault();
                //Get Job Detail...!!!
                string touserid = keys.EncryptString(jobstatus.toUserId.ToString());
                string tojobid = keys.EncryptString(job.Pk_JobId.ToString());
                string paymenturlClient = "http://hardyhat.com/user/payment/checkout?userid=" + touserid + "&jobid=" + tojobid;
                MailMessage messageClient = new MailMessage(
               "info@hardyhat.com", // From field
               touserdetail.EmailId, // Recipient field
               "Pending For Payment Approval", // Subject of the email message
               PopulateBody(clsSession.UserName, job.JobTitle, job.JobCategory, job.JobLocation, "$" + job.Amount.ToString(), job.JobDescription, false, paymenturlClient, false, true) // Email message body
               );
                _sendemail.SendEmail(messageClient);
            }
            return RedirectToAction("jobproposals");
        }
        public ActionResult reject(Int32 id)
        {
            tbl_JobEmployees jobstatus = _dataContext.tbl_JobEmployees.Where(s => s.Fk_JobId == id).FirstOrDefault();
            jobstatus.EmplyeeFeePaymentStatus = "Rejected";
            _dataContext.SaveChanges();
            return RedirectToAction("jobproposals");
        }
        public ActionResult findwork()
        {
            var lstJobs = _dataContext.tbl_Jobs.Where(x => x.IsActive == true && x.JobStatus == "Pending").ToList();
            return View(lstJobs);
        }
        public ActionResult readmore(Int32 id)
        {
            var Job = _dataContext.tbl_Jobs.Where(x => x.IsActive == true && x.Pk_JobId == id).FirstOrDefault();
            var jobempdetaiil = _dataContext.tbl_JobEmployees.Where(s => s.Fk_JobId == id && s.fromUserId == clsSession.UserID).ToList();
            if (jobempdetaiil.Count > 0)
            {
                TempData["HideApply"] = true;
            }
            return View(Job);
        }
        public ActionResult applyjob(Int32 id)
        {
            tbl_Jobs job = _dataContext.tbl_Jobs.Where(s => s.Pk_JobId == id).SingleOrDefault();
            if (job != null)
            {
                tbl_JobEmployees employeedata = new tbl_JobEmployees();
                employeedata.Fk_JobId = id;
                employeedata.fromUserId = clsSession.UserID;
                employeedata.fromUserName = clsSession.UserName;
                employeedata.toUserId = job.ClientUserId;
                employeedata.EmplyeeFeePaymentStatus = "employee requested";
                employeedata.IsActive = true;
                employeedata.CreatedDate = DateTime.Now;
                _dataContext.tbl_JobEmployees.Add(employeedata);
                _dataContext.SaveChanges();
                //Sent Email to employee to introduce about job detail...!!!
                //Get send user details...!!!
                tbl_Users touserdetail = _dataContext.tbl_Users.Where(s => s.Pk_UserId == job.ClientUserId).FirstOrDefault();
                //Get Job Detail...!!!
                MailMessage message = new MailMessage(
               "info@hardyhat.com", // From field
               touserdetail.EmailId, // Recipient field
               "Job Request Sent by Client", // Subject of the email message
               PopulateBody(clsSession.UserName, job.JobTitle, job.JobCategory, job.JobLocation, "$" + job.Amount.ToString(), job.JobDescription, false, "", false, false) // Email message body
               );
                _sendemail.SendEmail(message);
            }
            return RedirectToAction("jobrequested");
        }
        public ActionResult viewjobrequested(Int32 id)
        {
            var jobDetails = _dataContext.tbl_Jobs.Where(s => s.Pk_JobId == id).FirstOrDefault();
            return View(jobDetails);
        }
        public ActionResult viewjobproposal(Int32 id, Guid userid)
        {
            userjob userjobdetails = new userjob();
            var job = _dataContext.tbl_Jobs.Where(s => s.Pk_JobId == id).FirstOrDefault();
            var userdetails = _dataContext.tbl_Users.Where(s => s.Pk_UserId == userid).FirstOrDefault();
            userjobdetails.tbl_job = job;
            userjobdetails.tbl_uses = userdetails;
            return View(userjobdetails);
        }
        public ActionResult approvejob(Int32 id)
        {
            tbl_JobEmployees jobstatus = _dataContext.tbl_JobEmployees.Where(s => s.Fk_JobId == id).FirstOrDefault();
            jobstatus.EmplyeeFeePaymentStatus = "Approved";
            _dataContext.SaveChanges();
            //For Change Job Status...!!!
            tbl_Jobs job = _dataContext.tbl_Jobs.Where(s => s.Pk_JobId == id).FirstOrDefault();
            List<tbl_JobEmployees> jobemployeedata = _dataContext.tbl_JobEmployees.Where(s => s.Fk_JobId == id && s.EmplyeeFeePaymentStatus == "Approved").ToList();
            if (jobemployeedata.Count >= job.NoOfEmployeeNeeded)
            {
                job.JobStatus = "Approved";
                _dataContext.SaveChanges();
            }
            //Sent Email to employee to introduce about job detail...!!!
            //Get send user details...!!!
            tbl_Users touserdetail = _dataContext.tbl_Users.Where(s => s.Pk_UserId == jobstatus.fromUserId).FirstOrDefault();
            string userid = keys.EncryptString(clsSession.UserID.ToString());
            string jobid = keys.EncryptString(job.Pk_JobId.ToString());
            string paymenturl = "http://hardyhat.com/user/payment/checkout?userid=" + userid + "&jobid=" + jobid;
            //Get Job Detail...!!!
            MailMessage message = new MailMessage(
           "info@hardyhat.com", // From field
           touserdetail.EmailId, // Recipient field
           "Job Approved by Client", // Subject of the email message
           PopulateBody(clsSession.UserName, job.JobTitle, job.JobCategory, job.JobLocation, "$" + job.Amount.ToString(), job.JobDescription, true, paymenturl, false, false) // Email message body
           );
            return View("clientjobproposal");
        }
        public ActionResult rejectjob(Int32 id)
        {
            tbl_JobEmployees jobstatus = _dataContext.tbl_JobEmployees.Where(s => s.Fk_JobId == id).FirstOrDefault();
            jobstatus.EmplyeeFeePaymentStatus = "Rejected";
            _dataContext.SaveChanges();
            return View("clientjobproposal");
        }
        private string PopulateBody(string employeename, string jobtitle, string jobcategory, string joblocation, string amountpay, string jobdescription, bool isClient, string paymenturl, bool isEmployee, bool isPaymentClient)
        {
            string body = string.Empty;
            if (isPaymentClient)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/" + "jobpaymentforclient.html")))
                {
                    body = reader.ReadToEnd();
                }
            }
            if (isClient)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/" + "jobapprovedclient.html")))
                {
                    body = reader.ReadToEnd();
                }
            }
            if (isEmployee)
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/" + "jobapprovedemployee.html")))
                {
                    body = reader.ReadToEnd();
                }
            }
            else
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/" + "jobemployeerequest.html")))
                {
                    body = reader.ReadToEnd();
                }
            }
            if (isClient)
            {
                body = body.Replace("{{client_name}}", employeename);
                body = body.Replace("{{payment_url}}", paymenturl);
            }
            if (isEmployee)
            {
                body = body.Replace("{{payment_url}}", paymenturl);
            }
            if (isPaymentClient)
            {
                body = body.Replace("{{payment_url}}", paymenturl);
            }
            else
            {
                body = body.Replace("{{employee_name}}", employeename);
            }
            body = body.Replace("{{job_title}}", jobtitle);
            body = body.Replace("{{job_category}}", jobcategory);
            body = body.Replace("{{job_location}}", joblocation);
            body = body.Replace("{{amount_pay}}", amountpay);
            body = body.Replace("{{job_description}}", jobdescription);
            return body;
        }

    }
}