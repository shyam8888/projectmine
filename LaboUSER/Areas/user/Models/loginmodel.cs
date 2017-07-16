using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace LaboUSER.Areas.user.Models
{
    public class loginmodel
    {
        [Required(ErrorMessage="Email is required.")]
        [EmailAddress(ErrorMessage="Enter valid email address.")]
        public string email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "Please enter at least 8 characters.")]
        public string password { get; set; }
    }
}