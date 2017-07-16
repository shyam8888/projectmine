using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaboUSER.Areas.user.Controllers
{
    public class errorController : Controller
    {
        //
        // GET: /user/error/

        public ActionResult notfound()
        {
            return View();
        }

    }
}
