using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaboUSER.Areas.user.Controllers
{
    public class AdminSettingController : Controller
    {
        //
        // GET: /user/AdminSetting/

        public ActionResult Index()
        {
            return View("~/Areas/user/Views/AdminSetting/AdminSettingView.cshtml");
        }

    }
}