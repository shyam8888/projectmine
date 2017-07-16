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

namespace LaboUSER.Areas.user.Controllers
{
    [SessionCheckFilter]
    public class DashboardController : Controller
    {
        //
        // GET: /user/Dashboard/
        private readonly LABOEntities _dataContext;
        public readonly SentEmailController _sendemail;
        public DashboardController()
        {
            _dataContext = new LABOEntities();
            _sendemail = new SentEmailController();
        }

        public ActionResult admin()
        {
            var userDetails = _dataContext.dashboard_count("Admin", null).FirstOrDefault();
            return View(userDetails);
        }
        public ActionResult client()
        {
            return View();
        }
        public ActionResult employee()
        {
            return View();
        }
    }
}
