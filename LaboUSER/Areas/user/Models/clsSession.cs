using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace LaboUSER.Areas.user
{
    public class clsSession
    {
        public static string paymentID
        {
            get
            {
                return HttpContext.Current.Session["paymentID"] != null ? Convert.ToString(HttpContext.Current.Session["paymentID"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["paymentID"] = value;
            }
        }
        public static Guid UserID
        {
            get
            {
                return HttpContext.Current.Session["UserID"] != null ? new Guid(Convert.ToString(HttpContext.Current.Session["UserID"])) : Guid.Empty;
            }
            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }
        public static Int32 CityID
        {
            get
            {
                return HttpContext.Current.Session["CityID"] != null ? Convert.ToInt32(Convert.ToString(HttpContext.Current.Session["CityID"])) : 0;
            }
            set
            {
                HttpContext.Current.Session["CityID"] = value;
            }
        }
        public static Guid RoleID
        {
            get
            {
                return HttpContext.Current.Session["RoleID"] != null ? new Guid(Convert.ToString(HttpContext.Current.Session["RoleID"])) : Guid.Empty;
            }
            set
            {
                HttpContext.Current.Session["RoleID"] = value;
            }
        }
        public static String SessionID
        {
            get
            {
                return HttpContext.Current.Session["SessionID"] != null ? Convert.ToString(HttpContext.Current.Session["SessionID"]) : String.Empty;
            }
            set
            {
                HttpContext.Current.Session["SessionID"] = value;
            }
        }
        public static String RoleName
        {
            get
            {
                return HttpContext.Current.Session["RoleName"] != null ? Convert.ToString(HttpContext.Current.Session["RoleName"]) : String.Empty;
            }
            set
            {
                HttpContext.Current.Session["RoleName"] = value;
            }
        }
        public static String Language
        {
            get
            {
                return HttpContext.Current.Session["Language"] != null ? Convert.ToString(HttpContext.Current.Session["Language"]) : String.Empty;
            }
            set
            {
                HttpContext.Current.Session["Language"] = value;
            }
        }
        public static string Designation
        {
            get
            {
                return HttpContext.Current.Session["Designation"] != null ? Convert.ToString(HttpContext.Current.Session["Designation"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["Designation"] = value;
            }
        }
        public static string ImagePath
        {
            get
            {
                return HttpContext.Current.Session["ImagePath"] != null ? Convert.ToString(HttpContext.Current.Session["ImagePath"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["ImagePath"] = value;
            }
        }
        public static string DepartmentName
        {
            get
            {
                return HttpContext.Current.Session["DepartmentName"] != null ? Convert.ToString(HttpContext.Current.Session["DepartmentName"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["DepartmentName"] = value;
            }
        }
        public static string UserName
        {
            get
            {
                return HttpContext.Current.Session["UserName"] != null ? Convert.ToString(HttpContext.Current.Session["UserName"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }

       
    }
}
