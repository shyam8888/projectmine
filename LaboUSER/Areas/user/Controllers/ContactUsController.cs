using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Models;
using LaboUSER.Areas.user.Filters;
namespace LaboUSER.Areas.user.Controllers
{
    [SessionCheckFilter]
    public class ContactUsController : Controller
    {
        //
        // GET: /user/ContactUs/
        private readonly LABOEntities _dataContaxt;
        public ContactUsController()
        {
            _dataContaxt = new LABOEntities();
        }
        public ActionResult Index()
        {
            var list = _dataContaxt.tbl_ContactUs.ToList();
            return View(list);
        }
        public ActionResult view(Int32 id)
        {
            var contactusinfo = _dataContaxt.tbl_ContactUs.Find(id);
            return View(contactusinfo);
        }
    }
}
