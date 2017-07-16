using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LaboUSER.Models;
namespace LaboUSER.Areas.user.Filters
{
    public class SessionCheckFilter : FilterAttribute, IActionFilter
    {


        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (clsSession.SessionID == "" || clsSession.SessionID != HttpContext.Current.Session.SessionID)
                {
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = "~/Areas/user/Views/Login/Login.cshtml"
                    };
                }
            }
            else
            {
                if (clsSession.SessionID == "" || clsSession.SessionID != HttpContext.Current.Session.SessionID)
                {
                    filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary 
                                   {
                                       { "action", "Login" },
                                       { "controller", "Login" }
                                   });
                }
            }

        }
    }

}