using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Models;
namespace LaboUSER.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        private readonly LABOEntities _dataContext;
        public ContactController(){
            _dataContext=new LABOEntities();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(tbl_ContactUs contactus)
        {
            //Save record into table
            contactus.IsActive = true;
            contactus.CreatedDate = DateTime.Now;
            contactus.ModifiedDate = DateTime.Now;
            _dataContext.tbl_ContactUs.Add(contactus);
            _dataContext.SaveChanges();
            return RedirectToRoute("ThankYou");
            //Send email to the user
        }

    }
}
