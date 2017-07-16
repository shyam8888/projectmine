using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Areas.user.Models;
using LaboUSER.Models;
namespace LaboUSER.Areas.user.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /user/Login/
        private readonly LABOEntities _dataContext;
        public LoginController()
        {
            _dataContext = new LABOEntities();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(loginmodel userLogin)
        {
            var model = _dataContext.tbl_Users.Where(s => s.EmailId == userLogin.email && s.Password == userLogin.password && s.IsActive==true && s.IsBlocked==false).SingleOrDefault();
            if (model == null)
            {
                TempData["LoginError"] = "Invalid username or password";
                return View();
            }
            else
            {
                clsSession.SessionID = Session.SessionID;
                clsSession.UserID = model.Pk_UserId;
                clsSession.RoleID = new Guid(model.Fk_RoleId.ToString());
                clsSession.UserName = model.FirstName + " " + model.LastName;
                clsSession.CityID = Convert.ToInt32(model.Fk_CityId);
                clsSession.ImagePath = model.UserImage;
                tbl_Roles role = _dataContext.tbl_Roles.Where(s => s.Pk_RoleId == model.Fk_RoleId).FirstOrDefault();
                clsSession.RoleName = role.RoleName;
                if (role.RoleName == "Employee")
                {
                    return RedirectToRoute("Employee_Home");
                }
                if (role.RoleName == "Client")
                {
                    return RedirectToRoute("Client_Home");
                }
                if (role.RoleName == "Admin")
                {
                    return RedirectToRoute("Admin_Home");
                }

                return RedirectToRoute("Employee_Home");
            }
        }
        public ActionResult Signout()
        {
            clsSession.SessionID = "";
            return RedirectToAction("Login");
        }

    }
}
