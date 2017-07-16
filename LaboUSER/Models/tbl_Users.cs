//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaboUSER.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class tbl_Users
    {
        public System.Guid Pk_UserId { get; set; }
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Middle name is required")]
        public string MidleName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email address.")]
        [System.Web.Mvc.Remote("IsEmail_Available", "Register", ErrorMessage = "Email id is already exist")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Contact number is required")]
        [System.Web.Mvc.Remote("IsContact_Available", "Register", ErrorMessage = "Contact no. is already exist")]
        public string ContactNo { get; set; }
        public string CompanyName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string UserImage { get; set; }
        public Nullable<System.Guid> Fk_RoleId { get; set; }
        [Required(ErrorMessage = "Government Id is required.")]
        public string Gov_IssueId { get; set; }
        [Required(ErrorMessage = "Eligible work Id is required.")]
        public string EligibleWorkId { get; set; }
        [Required(ErrorMessage = "Police verification is required.")]
        public string PoliceVerification { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        [Required(ErrorMessage = "Postal code is required")]
        public string ZipCode { get; set; }
        public string UserType { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsBlocked { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Confirm password should be match with password.")]
        public string cpassword { get; set; }
        [Required(ErrorMessage = "Accept terms & condition")]
        public bool terms { get; set; }

        public Nullable<long> Fk_CountryId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public Nullable<long> Fk_StateId { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public Nullable<long> Fk_CityId { get; set; }
    }
}