using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboUSER.Models;
using LaboUSER.Areas.user.Filters;
using System.Data.Entity.Validation;
using System.Text;
namespace LaboUSER.Areas.user.Controllers
{
    [SessionCheckFilter]
    public class ClientListController : Controller
    {
        //
        // GET: /ClientList/
        private readonly LABOEntities _dataContext;

        public ClientListController()
        {
            _dataContext = new LABOEntities();
        }

        public ActionResult Index()
        {
            // check client role name
            tbl_Roles role = _dataContext.tbl_Roles.Where(x => x.RoleName == "Client").FirstOrDefault();
            List<tbl_Users> lstUsers = _dataContext.tbl_Users.Where(x => x.Fk_RoleId == role.Pk_RoleId).ToList();
            return View(lstUsers);
        }
        public ActionResult View(Guid? id)
        {
            //check view client role
            tbl_Roles role = _dataContext.tbl_Roles.Where(x => x.RoleName == "Client").FirstOrDefault();
            tbl_Users client = _dataContext.tbl_Users.Where(x => x.Fk_RoleId == role.Pk_RoleId && x.Pk_UserId == id).FirstOrDefault();

            return View("~/Areas/user/Views/ClientList/ViewClient.cshtml", client);

        }
        public ActionResult activate(Guid id)
        {
            tbl_Users user = _dataContext.tbl_Users.Where(s => s.Pk_UserId == id).SingleOrDefault();
            if (user != null)
            {
                user.IsActive = !(user.IsActive);
                user.cpassword = user.Password;
                if (ModelState.IsValid)
                {
                    try
                    {
                        _dataContext.SaveChanges();
                    }
                    catch (DbEntityValidationException dbValEx)
                    {
                        var outputLines = new StringBuilder();
                        foreach (var eve in dbValEx.EntityValidationErrors)
                        {
                            outputLines.AppendFormat("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:"
                              , DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State);

                            foreach (var ve in eve.ValidationErrors)
                            {
                                outputLines.AppendFormat("- Property: \"{0}\", Error: \"{1}\""
                                 , ve.PropertyName, ve.ErrorMessage);
                            }
                        }

                        throw new DbEntityValidationException(string.Format("Validation errors\r\n{0}"
                         , outputLines.ToString()), dbValEx);
                    }
                }
            }
            tbl_Roles role = _dataContext.tbl_Roles.Where(x => x.RoleName == "Client").FirstOrDefault();
            List<tbl_Users> lstUsers = _dataContext.tbl_Users.Where(x => x.Fk_RoleId == role.Pk_RoleId).ToList();
            return View("~/Areas/user/Views/ClientList/Index.cshtml", lstUsers);
        }
        public ActionResult block(Guid id)
        {
            tbl_Users user = _dataContext.tbl_Users.Where(s => s.Pk_UserId == id).SingleOrDefault();
            if (user != null)
            {
                user.IsBlocked = !(user.IsBlocked);
                user.cpassword = user.Password;
                if (ModelState.IsValid)
                {
                    try
                    {
                        _dataContext.SaveChanges();
                    }
                    catch (DbEntityValidationException dbValEx)
                    {
                        var outputLines = new StringBuilder();
                        foreach (var eve in dbValEx.EntityValidationErrors)
                        {
                            outputLines.AppendFormat("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:"
                              , DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State);

                            foreach (var ve in eve.ValidationErrors)
                            {
                                outputLines.AppendFormat("- Property: \"{0}\", Error: \"{1}\""
                                 , ve.PropertyName, ve.ErrorMessage);
                            }
                        }

                        throw new DbEntityValidationException(string.Format("Validation errors\r\n{0}"
                         , outputLines.ToString()), dbValEx);
                    }
                }

            }
            tbl_Roles role = _dataContext.tbl_Roles.Where(x => x.RoleName == "Client").FirstOrDefault();
            List<tbl_Users> lstUsers = _dataContext.tbl_Users.Where(x => x.Fk_RoleId == role.Pk_RoleId).ToList();
            return View("~/Areas/user/Views/ClientList/Index.cshtml", lstUsers);
        }
    }
}
