using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Areas.user.Filters;
namespace LaboUSER.Areas.user.Controllers
{
    [SessionCheckFilter]
    public class HomeController : Controller
    {
        //
        // GET: /user/Home/

        public ActionResult UserHome()
        {
            return View();
        }

    }
}
