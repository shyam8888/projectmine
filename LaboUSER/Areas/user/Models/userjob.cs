using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaboUSER.Models;
using LaboUSER.Areas.user.Filters;
namespace LaboUSER.Areas.user.Models
{
    public class userjob
    {
        public tbl_Users tbl_uses { get; set; }
        public tbl_Jobs tbl_job { get; set; }
    }
}